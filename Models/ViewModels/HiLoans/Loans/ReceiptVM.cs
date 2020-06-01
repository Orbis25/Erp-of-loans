using Models.Models.HiAccounting.Debs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels.HiLoans.Loans
{
    public class ReceiptVM
    {
        public string CompanyName { get; set; }
        public string Rnc { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Deb Deb { get; set; }
        public decimal Amount { get; set; }
    }
}
