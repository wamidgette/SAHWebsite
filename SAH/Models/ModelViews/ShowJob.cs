using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ShowJob
    {
        //Job data information
        public JobDto Job { get; set; }

        // Data about the application per Job
        public IEnumerable<ApplicationDto> Applications { get; set; }

    }
}