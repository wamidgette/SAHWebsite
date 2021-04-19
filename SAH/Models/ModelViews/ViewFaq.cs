using System.Collections.Generic;
using System.Web.Mvc;

namespace SAH.Models.ModelViews
{
    public class ViewFaq
    {
        //The list of FAQs
        public IEnumerable<FaqDto> FaqList { get; set; }

        //New FAQ submitted by public user
        public FaqDto newFaq { get; set; }

        //Department Information
        public SelectList DepartmentsSelectList { get; set; }

    }
}