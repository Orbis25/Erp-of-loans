using Models.Models.HiAccounting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels.HiLoans.Loans
{
    public class FilterLoanVM
    {
        public Guid? EnterpriseId { get; set; } = null;
        public Guid? BankId { get; set; } = null;
        public IEnumerable<Loan> Results { get; set; }
    }
}
