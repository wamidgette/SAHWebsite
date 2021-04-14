using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ChangeApplication
    {
        //Information about the application
        public ApplicationDto Application { get; set; }

        //Needed for a dropdownlist for users
        public IEnumerable<UserDto> AllUsers { get; set; }
        //Needed for a dropdownlist for Jobs
        public IEnumerable<JobDto> Jobs { get; set; }
    }
}