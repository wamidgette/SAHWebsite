using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ListJobs
    {
        public bool isadmin { get; set; }
        public IEnumerable<JobDto> joblist { get; set; }
    }
}