using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class Account
    {
        public int Id { get; set; }
        public Nullable<int> RoleId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name requires minimum 4 and maximum 50 characters", MinimumLength = 4)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password requires minimum 6 and maximum 50 characters", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string DOB { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Tel { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Avatar { get; set; }

        public virtual Role Role { get; set; }
        public ICollection<Account_Faculty> Account_Faculties { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}