using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ShowApplication
    {
        //Information about the Job
        public JobDto Job { get; set; }
        //User match the Job Applicatio
        public UserDto User { get; set; }
        //Information about the Job Application
        public ApplicationDto Application { get; set; }
    }
}