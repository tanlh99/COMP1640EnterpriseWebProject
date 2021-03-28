using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterpriseWebProject.Models
{
    public class Magazine
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Magazine Name requires minimum 8 and maximum 30 characters", MinimumLength = 8)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string OpenDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string CloseDate { get; set; }
        public bool Status { get; set; }
        public int AcademicYearID { get; set; }
        public string PDFfile { get; set; }
        public string ImgFile { get; set; }
        [Required]
        public string Description { get; set; }

        public ICollection<Magazine_Faculty> Magazine_Faculties { get; set; }
        public virtual AcademicYear AcademicYear { get; set; }
    }
}