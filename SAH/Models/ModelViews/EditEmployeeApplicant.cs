using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class EditEmployeeApplicant
    {

        //Information about the ticket
        public EmployeeApplicantDto EmployeeApplicant { get; set; }

        //Needed for a dropdownlist for users
        public IEnumerable<ApplicationUserDto> AllUsers { get; set; }
        //Needed for a dropdownlist for parking spots
        public IEnumerable<CoursesDto> AllCourses { get; set; }

        public bool isadmin { get; set; }

        public string userid { get; set; }
    }
}
