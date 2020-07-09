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
        public decimal Amount { get; set; }
    }

    public class ReportOfLootVM
    {
        public string Date { get; set; }
        public decimal Loot { get; set; }
        public string BankName { get; set; }
        public string BankNameNormalize => BankName.Trim() == "Ninguna" ? "N/A" : BankName;
        public string CompanyName { get; set; }
    }
}
