﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ListCourses
    {
        //Pass this flag to conditionally render update/new links
        public bool isadmin { get; set; }
        public IEnumerable<CoursesDto> courselist { get; set; }
    }
}