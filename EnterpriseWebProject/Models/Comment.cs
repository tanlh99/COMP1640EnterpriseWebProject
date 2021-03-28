using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string SubmitDate { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> ContributionId { get; set; }
        
        public virtual Contribution Contribution { get; set; }
        public virtual Account Account { get; set; }
    }
}