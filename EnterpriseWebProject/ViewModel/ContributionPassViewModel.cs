﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.ViewModel
{
    public class ContributionPassViewModel
    {
        public List<EnterpriseWebProject.Models.File> Files { get; set; }
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string FileName { get; set; }
        //public DateTime SubmitDate { get; set; }

        //public List<EnterpriseWebProject.Models.Magazine> Magazines { get; set; }
        public string MagazineName { get; set; }
        public int MagazineId { get; set; }

    }
}