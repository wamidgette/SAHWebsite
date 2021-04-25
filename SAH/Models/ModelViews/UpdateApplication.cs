using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class UpdateApplication
    {
        //Data about the Application
        public ApplicationDto Application { get; set; }
        //Shows all the Jobs offered by Sah in order to create a dropdown menu
        public IEnumerable<JobDto> Jobs { get; set; }

        //Shows all users in order to create a dropdown menu
        public IEnumerable<ApplicationUserDto> ApplicationUsers { get; set; }



    }
}