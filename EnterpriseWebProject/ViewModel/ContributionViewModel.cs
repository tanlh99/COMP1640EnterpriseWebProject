using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.ViewModel
{
    public class ContributionViewModel
    {
        public DateTime SubmitDate { get; set; }
        public bool Status { get; set; }
        public int AccountId { get; set; }

        public int MagazineId { get; set; }
        public int FacultyId { get; set; }


        public string Name { get; set; }
        public string FileDoc { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

    }
}