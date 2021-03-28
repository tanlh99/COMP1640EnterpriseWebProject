using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Minimum 4 and maximum 100 characters are required", MinimumLength = 4)]
        public string Name { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}