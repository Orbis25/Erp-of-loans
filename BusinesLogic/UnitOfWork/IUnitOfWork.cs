using BusinesLogic.Interfaces;
using BusinesLogic.Interfaces.Alerts;
using BusinesLogic.Interfaces.HiAccouting;
using BusinesLogic.Interfaces.HiLoans;
using BusinesLogic.Interfaces.Shared;
using BusinesLogic.Repository.Interfaces;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinesLogic.UnitOfWork
{
    public interface IUnitOfWork
    {
        IMovementService MovementsService { get; }
        IClientUserService ClientUserService { get; }
        IUserService UserService { get; }
        IHomeService HomeService { get; }
        IInventaryService InventaryService { get; }
        IEnterpriseService EnterpriseService { get; }
        ILoanService LoanService { get; }
        IAlertService AlertService { get; }
        IRoleService RoleService { get; }
        ICompanyService CompanyService { get; }
        IBankService BankService { get; }
        Task Commit();
    }
}
