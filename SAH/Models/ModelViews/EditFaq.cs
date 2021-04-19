using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAH.Models.ModelViews
{
    public class EditFaq
    {
        //Information about the FAQ
        public FaqDto Faq { get; set; }

        //Department Information
        public SelectList DepartmentsSelectList { get; set; }

    }
}