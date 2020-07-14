using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinesLogic.Interfaces;
using BusinesLogic.Interfaces.Alerts;
using BusinesLogic.Interfaces.HiAccouting;
using BusinesLogic.Interfaces.HiLoans;
using BusinesLogic.Interfaces.Shared;
using BusinesLogic.Repository.Interfaces;
using BusinesLogic.Repository.Services;
using BusinesLogic.Services;
using BusinesLogic.Services.Alerts;
using BusinesLogic.Services.HiAccouting;
using BusinesLogic.Services.HiLoans;
using BusinesLogic.Services.Shared;
using Models.Contexts;
using Models.Models;

namespace BusinesLogic.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ClientUserService _clientUserService;
        private UserService _userService;
        private MovementService _movementsService;
        private HomeService _homeService;
        private InventaryService _inventoryService;
        private EnterpriseService _enterpriseService;
        private LoanService _loanService;
        private AlertService _alertService;
        private RoleService _roleService;
        private CompanyService _companyService;
        private BankService _bankService;

        public UnitOfWork(ApplicationDbContext context) => _context = context;


        #region HILoans 
        public ILoanService LoanService => _loanService ?? (_loanService = new LoanService(_context));
        public IClientUserService ClientUserService => _clientUserService ?? (_clientUserService = new ClientUserService(_context));
        public IMovementService MovementsService => _movementsService ?? (_movementsService = new MovementService(_context));
        public IEnterpriseService EnterpriseService => _enterpriseService ?? (_enterpriseService = new EnterpriseService(_context));
        #endregion

        #region HIInventary
        public IInventaryService InventaryService => _inventoryService ?? (_inventoryService = new InventaryService(_context));
        #endregion

        #region Shared
        public IUserService UserService => _userService ?? (_userService = new UserService(_context));
        public IHomeService HomeService => _homeService ?? (_homeService = new HomeService(_context));

        public IAlertService AlertService => _alertService ?? (_alertService = new AlertService(_context));

        public IRoleService RoleService => _roleService ?? (_roleService = new RoleService(_context));

        public ICompanyService CompanyService => _companyService ?? (_companyService = new CompanyService(_context));

        public IBankService BankService => _bankService ?? (_bankService = new BankService(_context));

        async Task IUnitOfWork.Commit() => await _context.SaveChangesAsync();
        #endregion

    }
}

