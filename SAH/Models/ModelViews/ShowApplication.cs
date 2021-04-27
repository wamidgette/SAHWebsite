﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ShowApplication
    {
        //Data about the Job
        public JobDto Job { get; set; }
        //Data about the User
        public ApplicationUserDto ApplicationUser { get; set; }
        //Data information about Job Application
        public ApplicationDto Application { get; set; }
        public bool isadmin { get; set; }
    }
}