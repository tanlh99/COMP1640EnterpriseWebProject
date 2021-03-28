using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.ViewModel
{
    public class CommentViewModel
    {
        //Contribution model
        public int Number { get; set; }
        public DateTime SubmitDate { get; set; }
        public bool Status { get; set; }
        public string StudentName { get; set; }
        //File model
        public string Name { get; set; }
        public string FileDoc { get; set; }
        public string FileImg { get; set; }
        public string Description { get; set; }
        //Comment model
        public string Avatar { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime CommentDate { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> ContributionId { get; set; }
    }
}