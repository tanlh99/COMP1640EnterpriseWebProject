using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username requires minimum 4 and maximum 50 characters", MinimumLength = 4)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password requires minimum 6 and maximum 50 characters", MinimumLength = 6)]
        public string Password { get; set; }
        public bool rememberMe { get; set; }
    }
}