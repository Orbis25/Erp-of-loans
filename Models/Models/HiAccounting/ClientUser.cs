using Models.Models.Accounting;
using Models.Models.HiLoans;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Models
{
    /// <summary>
    /// A clientUser i was created by another clientUser , how by a admin
    /// </summary>
    public class ClientUser : CommonsProperty
    {
        public string UserId { get; set; }
        public User User { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public virtual IEnumerable<Movement> Movements { get; set; }
        public Guid EnterpriseId { get; set; }
        public Enterprise Enterprise { get; set; }
        public string IdentificationCard { get; set; }
        public string Address { get; set; }
        public Guid? BankId { get; set; }
        public Bank Bank { get; set; }
    }
}
