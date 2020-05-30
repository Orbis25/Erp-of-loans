using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels.HiLoans.Loans
{
    public class PendingClientVM
    {
        public Guid LoanId { get; set; }
        public string EnterpriseName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{Name} {LastName}";
        public string UserId { get; set; }
        public string AmountLoan { get; set; }
        public int TotalRate { get; set; }
    }
}
