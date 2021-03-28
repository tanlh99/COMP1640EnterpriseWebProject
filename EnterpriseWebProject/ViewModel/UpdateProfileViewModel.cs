using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.ViewModel
{
    public class UpdateProfileViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DOB { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
    }
}