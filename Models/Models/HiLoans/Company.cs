using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Models.HiLoans
{
    public class Company : CommonsProperty
    {
        [Display(Name = "Nombre de la empresa")]
        public string Name { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        public string Rnc { get; set; }
        [Display(Name = "Telefono")]
        [Phone]
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
