using Models.Models.HiAccounting;
using Models.Models.HiLoans;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels.HiLoans.Loans
{
   public class HistoryPaymentsLoanVM
    {
        public Loan Loan { get; set; }
        public virtual IEnumerable<HistoryPaymentsLoan> HistoryPaymentsLoans { get; set; }
    }
}
