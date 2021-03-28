using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.ViewModel
{
    public class Contri_FileViewModel
    {
        
        //File View Model
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int ContributionId { get; set; }
    }
}