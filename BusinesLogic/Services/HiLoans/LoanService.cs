﻿using BusinesLogic.Interfaces.HiLoans;
using BusinesLogic.Repository.Services;
using Microsoft.EntityFrameworkCore;
using Models.Contexts;
using Models.Enums;
using Models.Enums.HiAccounting;
using Models.Enums.HiLoans;
using Models.Models.HiAccounting;
using Models.Models.HiAccounting.Debs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.Services.HiLoans
{
    public class LoanService : BaseRepository<Loan>, ILoanService
    {
        public readonly ApplicationDbContext _dbContext;
        public LoanService(ApplicationDbContext dbContext) : base(dbContext)
            => _dbContext = dbContext;

        public override async Task<bool> Add(Loan model)
        {
            model.ActualCapital = model.InitialCapital;
            _dbContext.Loans.Add(model);
            var result = await _dbContext.SaveChangesAsync() > 0;
            if (result) await AddDebs(model, DateTime.Now);
            return result;
        }

        public async Task<IEnumerable<Loan>> GetAllWithRelationShip(string userId, Guid? idEnterprise = null)
        {
            var result = Filter(x => x.UserId == userId).Include(x => x.Debs)
                .Include(x => x.ClientUser.Enterprise)
                .Include(x => x.ClientUser)
                .ThenInclude(x => x.User)
            .AsQueryable();

            if (idEnterprise != null)
            {
                result = result.Where(x => x.ClientUser.EnterpriseId == idEnterprise);
            }

            return await result.OrderByDescending(x => x.CreateAt).ToListAsync();
        }

        public async Task<Loan> GetByIdWithRelationships(Guid id, State state)
        {
            var result = new Loan { };
            result = await GetAll().Include(x => x.ClientUser).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

            if (state == State.All) result.Debs = await _dbContext.Debs.Where(x => x.LoanId == id).ToListAsync();
            else result.Debs = await _dbContext.Debs.Where(x => x.LoanId == id && x.State == state).ToListAsync();

            var pendingsDebs = await _dbContext.Debs.CountAsync(x => x.State == State.Active && x.LoanId == id);
            var paymentDebs = await _dbContext.Debs.CountAsync(x => x.State == State.Payment && x.LoanId == id);
            result.SharesStr = pendingsDebs.ToString();
            return result;
        }

        public async Task<bool> PaymentDeb(Guid id, Guid idLoan, decimal extraMount)
        {
            var deb = await _dbContext.Debs.FirstOrDefaultAsync(x => x.Id == id);
            var loan = await _dbContext.Loans.FirstOrDefaultAsync(x => x.Id == idLoan);

            loan.ActualCapital = deb.State == State.Active ? loan.ActualCapital - (decimal)deb.Amortitation : loan.ActualCapital + (decimal)deb.Amortitation;
            _dbContext.Update(loan);

            await _dbContext.SaveChangesAsync();
            var result = false;
            if (deb.State == State.Active)
            {
                deb.ExtraMount = extraMount;
                deb.IsExtraMount = extraMount > 0;
                deb.State = State.Payment;
                _dbContext.Debs.Update(deb);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var extra = deb.ExtraMount;
                deb.ExtraMount = 0;
                deb.IsExtraMount = false;
                deb.State = State.Active;
                _dbContext.Debs.Update(deb);
                await _dbContext.SaveChangesAsync();
                if (extra > 0) await RecalculateDebs(idLoan, extra, false);
            }

            if (extraMount > 0)
            {
                _dbContext.Debs.Update(deb);
                await _dbContext.SaveChangesAsync();
                result = await RecalculateDebs(idLoan, extraMount);
            }
            return result;
        }

        public async Task<bool> SoftRemove(Guid id)
        {
            var model = await GetById(id);
            model.State = State.Removed;
            model.UpdateAt = DateTime.Now;
            return await Update(model);
        }

        private IEnumerable<Deb> FixedfeeDebs(Loan loan, DateTime lastDateTime, int count = 0)
        {
            double interest = (double)((decimal)loan.Interest) / 100;
            double monthly = interest;

            double shares = (loan.Shares - count);

            //decimal LoanDebsamortitation = 0;
            //if (loan.Debs !=null) {
            //    if (loan.Debs.Any()) LoanDebsamortitation = (decimal)loan.Debs.FirstOrDefault().Amortitation;

            //}

            decimal balance = loan.ActualCapital;
            if (loan.RateType == RateType.Anual) monthly = interest / 12;
            var payment = (double)loan.ActualCapital * (monthly / (1 - Math.Pow(1 + monthly, -shares)));
            var result = new List<Deb>();
            var nextPayment = lastDateTime;
            int debNumber = count;
            for (int i = 0; i < shares; i++)
            {
                debNumber++;
                var interestDeb = balance * (decimal)monthly;
                var deb = new Deb { };
                deb.Share = debNumber;
                nextPayment = GetDateOfPayment(loan.PaymentModality, nextPayment);
                deb.DateOfPayment = nextPayment;
                deb.Amount = balance;
                deb.Interest = interestDeb;
                deb.Amortitation = payment - (double)interestDeb;
                deb.EndBalance = balance - (decimal)deb.Amortitation;
                deb.ToPay = payment;
                deb.LoanId = loan.Id;
                balance -= (decimal)deb.Amortitation;
                result.Add(deb);
            }
            return result;
        }
        private IEnumerable<Deb> FixedInterestDebs(Loan loan, DateTime lastDateTime, int count = 0)
        {
            double interest = (double)((decimal)loan.Interest) / 100;
            double monthly = interest;
            int shares = loan.Shares - count;
            decimal LoanDebsamortitation = 0;
            decimal interestValue = 0;
            if (loan.Debs != null)
            {
                if (loan.Debs.Any())
                {
                    LoanDebsamortitation = (decimal)loan.Debs.FirstOrDefault().Amortitation;
                    interestValue = loan.Debs.FirstOrDefault().Interest;
                }
            }
            decimal balanceFixed = loan.ActualCapital;
            decimal balance = loan.ActualCapital;
            if (loan.RateType == RateType.Anual) monthly = interest / 12;
            var result = new List<Deb>();

            var nextPayment = lastDateTime;
            int debNumber = count;
            for (int i = 0; i < shares; i++)
            {
                debNumber++;
                //    var interestdeb = balanceFixed * (decimal)monthly;
                var interestdeb = interestValue == 0 ? (balanceFixed * (decimal)monthly) : interestValue;
                var monthlyPrincipal = balanceFixed / shares;
                nextPayment = GetDateOfPayment(loan.PaymentModality, nextPayment);
                double payment = (double)(monthlyPrincipal + interestdeb);
                var deb = new Deb
                {
                    Share = debNumber,
                    DateOfPayment = nextPayment,
                    Amount = balance,
                    Interest = interestdeb,
                    Amortitation = (double)monthlyPrincipal,
                    ToPay = payment,
                    EndBalance = balance - monthlyPrincipal,
                    LoanId = loan.Id
                };
                balance -= monthlyPrincipal;
                result.Add(deb);
            }
            return result;
        }
        private IEnumerable<Deb> CapitalEndDebs(Loan loan, DateTime lastDateTime, int count = 0)
        {
            double interest = (double)((decimal)loan.Interest) / 100;
            double monthly = interest;
            int shares = loan.Shares - count;
            decimal LoanDebsamortitation = 0;

            decimal interestValue = 0;
            if (loan.Debs != null)
            {
                if (loan.Debs.Any())
                {
                    LoanDebsamortitation = (decimal)loan.Debs.FirstOrDefault().Amortitation;
                    interestValue = loan.Debs.FirstOrDefault().Interest;
                }
            }

            decimal balance = loan.ActualCapital - LoanDebsamortitation;
            if (loan.RateType == RateType.Anual) monthly = interest / 12;
            var result = new List<Deb>();
            var nextPayment = lastDateTime;
            int debNumber = count;
            for (int i = 0; i < shares; i++)
            {
                debNumber++;
                var interestdeb = interestValue == 0 ? (balance * (decimal)monthly) : interestValue;
                var payment = interestdeb;
                decimal monthlyPrincipal = 0;


                if (i == (shares - 1))
                {
                    monthlyPrincipal = balance;
                    payment = balance + interestdeb;
                }

                var deb = new Deb
                {
                    Share = debNumber,
                    DateOfPayment = nextPayment,
                    Interest = interestdeb,
                    Amount = balance,
                    Amortitation = (double)monthlyPrincipal,
                    EndBalance = balance - monthlyPrincipal,
                    ToPay = (double)payment,
                    LoanId = loan.Id
                };
                result.Add(deb);
            }
            return result;
        }
        private DateTime GetDateOfPayment(PaymentModality modality, DateTime dateNow)
        {
            var date = dateNow;
            switch (modality)
            {
                case PaymentModality.Daily:
                    date = date.AddDays(1);
                    return date;
                case PaymentModality.Week:
                    date = date.AddDays(7);
                    return date;
                case PaymentModality.Biweekly:
                    date = date.AddDays(15);
                    return date;
                case PaymentModality.Month:
                    date = date.AddMonths(1);
                    return date;

                default: return DateTime.Now.AddYears(1);
            }
        }

        private async Task AddDebs(Loan model, DateTime lastDateTime, int count = 0)
        {
            IEnumerable<Deb> debs;
            if (model.AmortitationType == AmortitationType.Fixedfee)
            {
                debs = FixedfeeDebs(model, lastDateTime, count);
            }
            else if (model.AmortitationType == AmortitationType.FixedInterest)
            {
                debs = FixedInterestDebs(model, lastDateTime, count);
            }
            else debs = CapitalEndDebs(model, lastDateTime, count);

            _dbContext.Debs.AddRange(debs);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> RemoveAllDebs(IEnumerable<Deb> debs)
        {
            _dbContext.Debs.RemoveRange(debs);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        private async Task<bool> RecalculateDebs(Guid idLoan, decimal extraMount, bool isDiscount = true)
        {
            var result = false;
            var debs = await _dbContext.Debs.Where(x => x.State == State.Active && x.LoanId == idLoan).ToListAsync();
            var count = await _dbContext.Debs.CountAsync(x => x.State == State.Payment && x.LoanId == idLoan);
            var lastPayment = await _dbContext.Debs.Where(x => x.State == State.Payment && x.LoanId == idLoan)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            if (await RemoveAllDebs(debs))
            {
                var loan = await GetById(idLoan);
                loan.ActualCapital = isDiscount ? loan.ActualCapital - extraMount : loan.ActualCapital + extraMount;
                _dbContext.Loans.Update(loan);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    var dateOfPayment = DateTime.Now;
                    if (lastPayment != null) dateOfPayment = lastPayment.DateOfPayment;
                    await AddDebs(loan, dateOfPayment, count);
                    result = true;
                }
            }
            return result;
        }

        public IEnumerable<Deb> GetAmortization(Loan model)
        {
            IEnumerable<Deb> debs;
            if (model.AmortitationType == AmortitationType.Fixedfee)
            {
                debs = FixedfeeDebs(model, DateTime.Now);
            }
            else if (model.AmortitationType == AmortitationType.FixedInterest)
            {
                debs = FixedInterestDebs(model, DateTime.Now);
            }
            else debs = CapitalEndDebs(model, DateTime.Now);
            return debs;
        }
    }
}
