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
        public ICollection<ReportOfLootVM> Results { get; set; }
    }

    public class ReportOfLootVM
    {
        public DateTime Date { get; set; }
        public decimal Loot { get; set; }
        public string CompanyName { get; set; }
    }
}
