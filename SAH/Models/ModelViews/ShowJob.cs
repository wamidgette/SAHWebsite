using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ShowJob
    {
        //Information about the job
        public JobDto Job { get; set; }

        //Information about all application for the Job

        public IEnumerable<ApplicationDto> Applications { get; set; }

    }
}