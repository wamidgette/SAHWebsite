using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ShowEmployeeApplicant
    {
        //Information about the Courses
        public CoursesDto Courses { get; set; }
        //User match the Courses and application
        public UserDto User { get; set; }
        //Information about the Job Application
        public EmployeeApplicantDto EmployeeApplicant { get; set; }
    }
}