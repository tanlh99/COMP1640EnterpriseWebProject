using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public int ContributionId { get; set; }

        public virtual Contribution Contribution { get; set; }

    }
}