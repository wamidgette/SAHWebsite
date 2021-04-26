using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAH.Models
{
    public class Courses
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }

        [Display(Name = "Start Date dd/mm/yyyy")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter the start date for the course")]
        public DateTime StartOn { get; set; }

        public string CourseDuration { get; set; }
        //A course can have many applicants
        public ICollection<EmployeeApplicant> EmployeeApplicant { get; set; }
    }

    public class CoursesDto
    {
        [DisplayName("Course ID")]
        public int CourseId { get; set; }
        [DisplayName("Course Code")]
        public string CourseCode { get; set; }
        [DisplayName("Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartOn { get; set; }

        [DisplayName("Course Duration")]
        public string CourseDuration { get; set; }
        //number of applications for respective course
        public int NumApplications { get; set; }
    }
}