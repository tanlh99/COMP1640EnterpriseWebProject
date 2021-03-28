using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.ViewModel
{
    public class AdminEditAccViewModel
    {
        public int? NewRole { get; set; }
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password requires minimum 6 and maximum 50 characters", MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}