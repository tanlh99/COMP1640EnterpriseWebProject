using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class Contribution
    {
        public int Id { get; set; }
        public DateTime SubmitDate { get; set; }
        public bool Status { get; set; }
        public int AccountId { get; set; }
        public int MagazineId { get; set; }
        public int FacultyId { get; set; }
        

        public virtual Account Account { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public virtual Magazine_Faculty Magazine_Faculty { get; set; }
    }
}