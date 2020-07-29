using Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels.HiLoans.Loans
{
    public class FilterOfReportVM
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Guid? Enterprise { get; set; } = null;
        public Guid? Bank { get; set; } = null;
        public IEnumerable<BankResume> Banks { get; set; }
        public ICollection<ReportOfLootVM> Results { get; set; }
        public State DebState { get; set; } = State.Payment;
    }

    public class BankResume
    {
        public string BankName { get; set; }
        public string BankNameNormalize => BankName.Trim() == "Ninguna" ? "N/A" : BankName;
        public decimal Interest { get; set; }
        public decimal Payments { get; set; }
        public decimal Amount => Interest + Payments;

    }

    public class ReportOfLootVM
    {
        public string Date { get; set; }

        public decimal Debs { get; set; }
        public decimal OnlyInterest { get; set; }
        public decimal Loot => Debs + OnlyInterest;
        public string BankName { get; set; }
        public string BankNameNormalize => BankName.Trim() == "Ninguna" ? "N/A" : BankName;
        public string CompanyName { get; set; }
    }
}
