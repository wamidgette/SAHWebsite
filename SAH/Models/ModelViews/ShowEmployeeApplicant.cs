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
        //Information about the Courses of the application
        public UserDto User { get; set; }
        //Information about the Application
        public EmployeeApplicantDto EmployeeApplicant { get; set; }
        //Information about the Role of User
        public RoleDto Role { get; set; }
    }
}