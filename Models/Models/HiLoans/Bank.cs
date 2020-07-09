using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Models.HiLoans
{
    public class Bank : CommonsProperty
    {
        [Required]
        public string Name { get; set; }
    }
}
