using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class Account_Faculty
    {
        [Key]
        [Column(Order = 1)]
        public int AccountId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int FacultyId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
}