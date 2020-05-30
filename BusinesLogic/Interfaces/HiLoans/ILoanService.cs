using BusinesLogic.Repository.Interfaces;
using Models.Enums;
using Models.Models;
using Models.Models.HiAccounting;
using Models.Models.HiAccounting.Debs;
using Models.Models.HiLoans;
using Models.ViewModels.HiLoans.Loans;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.Interfaces.HiLoans
{
    public interface ILoanService : IBaseRepository<Loan>
    {
        Task<IEnumerable<Loan>> GetAllWithRelationShip(string userId, Guid? idEnterprise = null);
        Task<Loan> GetByIdWithRelationships(Guid id, State state);
        Task<bool> SoftRemove(Guid id);
        Task<bool> PaymentDeb(Guid id, Guid idLoan, decimal extraMount,bool InterestOnly);
        IEnumerable<Deb> GetAmortization(Loan model);
        Task<bool> AddReclosing(Loan model);
        int getShares(Loan model);
        Task<IEnumerable<ReclosingHistory>> GetReclosing(Guid id);
        Task<ICollection<PendingClientVM>> GetPaymentPendingClients(string createdBy);
        Task<List<MonthLoanVm>> GetLoanByMonth(string userId);
        Task<object> GetBadAndGoodClientPayments(string userId);
    }
}
