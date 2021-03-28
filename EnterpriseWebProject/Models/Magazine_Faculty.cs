using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class Magazine_Faculty
    {
        [Key,Column(Order = 1)]
        public int MagazineId { get; set; }
        [Key, Column(Order = 2)]
        public int FacultyId { get; set; }

        public virtual Magazine Magazine { get; set; }
        public virtual Faculty Faculty { get; set; }
        public ICollection<Contribution> Contributions { get; set; }
    }
}