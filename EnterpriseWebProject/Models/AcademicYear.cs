using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class AcademicYear
    {
        public int Id { get; set; }
        [Required]
        public int YearNo { get; set; }

        public ICollection<Magazine> Magazines { get; set; }
    }
}