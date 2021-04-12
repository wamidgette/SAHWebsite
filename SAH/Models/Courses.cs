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
    }

    public class CoursesDto
    {
        [DisplayName("Course ID")]
        public int CourseId { get; set; }
        [DisplayName("Course Code")]
        public string CourseCode { get; set; }
        [DisplayName("Course Name")]
        public string CourseName { get; set; }
    }
}