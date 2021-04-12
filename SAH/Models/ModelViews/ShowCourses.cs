using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ShowCourses
    {
        //Information about the job
        public CoursesDto Courses { get; set; }

        //Information about all application for the Job

        public IEnumerable<EmployeeApplicantDto> EmployeeApplications { get; set; }

    }
}