using Models.Enums;
using Models.Models;
using Models.Models.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.ClientUsers
{
    public class CreateUserViewModel : User
    {
        [Required]
        public RolsAuthorization Rol { get; set; }
        [Required]
        public Guid EnterpriseId { get; set; }
        public IEnumerable<Enterprise> Enterprises { get; set; }
        public string IdentificationCard { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Institución financiera de cobro")]
        public Guid? BankId { get; set; } = null;
    }
}
