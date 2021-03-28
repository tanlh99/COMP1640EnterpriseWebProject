using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Minimum 2 and maximum 100 characters are required", MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public ICollection<Account_Faculty> Account_Faculties { get; set; }
        public ICollection<Magazine_Faculty> Magazine_Faculties { get; set; }
    }
}