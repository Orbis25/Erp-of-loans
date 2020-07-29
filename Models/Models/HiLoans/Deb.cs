using Commons.Helpers;
using Models.Models.HiLoans;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace Models.Models.HiAccounting.Debs
{
    public class Deb : CommonsProperty
    {
        public DateTime DateOfPayment { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Interest { get; set; }
        [Required]
        public double ToPay { get; set; }
        public int Share { get; set; }
        public double Amortitation { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal EndBalance { get; set; }
        [Column(TypeName = "decimal(18,2)")]

        public decimal ExtraMount { get; set; }
        public bool IsExtraMount { get; set; } = false;
        public bool AllowPayInterest { get; set; } = false;
        public string Note { get; set; }
        public Guid LoanId { get; set; }
        public Loan Loan { get; set; }

        [NotMapped]
        public string StateStr => State == Enums.State.Payment ? "Pagado" : (State == Enums.State.Active && DateOfPayment < DateTime.Now) ? "Atrasada"  : "Pendiente";
        [NotMapped]
        public string ExtraMountStr => ExtraMount == 0 ? "N/A" : ExtraMount.ToString();
        //Formated property
        [NotMapped]
        public string DateOfPaymentFormated => StringHelper.FormatDate(DateOfPayment);

        [NotMapped]
        public string AmountFormated => StringHelper.FormatMoney(Amount);

        [NotMapped]
        public string InterestFormated => StringHelper.FormatMoney(Interest);

        [NotMapped]
        public string ToPayFormated => StringHelper.FormatMoney((decimal)ToPay);

        [NotMapped]
        public string AmortitationFormated => StringHelper.FormatMoney((decimal)Amortitation);

        [NotMapped]
        public string EndBalanceFormated => StringHelper.FormatMoney(EndBalance);

        [NotMapped]
        public string ExtraMountFormated => StringHelper.FormatMoney(ExtraMount);
    }

}
