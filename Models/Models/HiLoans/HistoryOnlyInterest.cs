using Models.Models.HiAccounting;
using Models.Models.HiAccounting.Debs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Models.HiLoans
{
    public class HistoryOnlyInterest : CommonsProperty
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public Guid LoanId { get; set; }
        public Loan Loan { get; set; }
    }
}
