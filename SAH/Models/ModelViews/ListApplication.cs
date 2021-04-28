using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ListApplication
    {
        public bool isadmin { get; set; }       
       
        public IEnumerable<ShowApplication> Applications { get; set; }

    }
}