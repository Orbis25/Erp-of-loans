using Commons.Helpers;
using Models.Models.HiAccounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Models.HiLoans
{
    public  class HistoryPaymentsLoan : CommonsProperty
    {
        public int Share { get; set; }
        public int outstanding { get; set; }
        [Required]
        public double ToPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal EndBalance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExtraMount { get; set; }
        public Guid IdLoan { get; set; }
        public Loan Loan { get; set; }
        [NotMapped]
        public string StateStr => State == Enums.State.Payment ? "Realizado" : (State == Enums.State.OnlyInterest ) ? "Solo Interes" : "Anulado";
        [NotMapped]
        public string DateOfPaymentFormated => StringHelper.FormatDate(CreateAt);
        [NotMapped]
        public string ExtraMountFormated => StringHelper.FormatMoney(ExtraMount);
        [NotMapped]
        public string ToPayFormated => StringHelper.FormatMoney((decimal)ToPay);
    }
}
