using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class changeApplication
    {
        //Information about the ticket
        public ApplicationDto Application { get; set; }

        //Needed for a dropdownlist for users
        public IEnumerable<UserDto> AllUsers { get; set; }
        //Needed for a dropdownlist for parking spots
        public IEnumerable<JobDto> Jobs { get; set; }
    }
}