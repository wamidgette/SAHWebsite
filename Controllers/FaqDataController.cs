using SAH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using static SAH.Models.IdentityModels;

namespace SAH.Controllers
{
    public class FaqDataController : ApiController
    {
        //This variable is our database access point
        private SAHDataContext db = new SAHDataContext();

        [ResponseType(typeof(IEnumerable<FaqDto>))]
        public IHttpActionResult GetFaqs()
        {
            List<Faq> faqs = db.Faqs.ToList();
            List<FaqDto> faqDtos = new List<FaqDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var faq in faqs)
            {
                FaqDto newFaq = new FaqDto
                {
                    FaqID = faq.FaqID,
                    Question = faq.Question,
                    Answer = faq.Answer
                };
                faqDtos.Add(newFaq);
            }

            return Ok(faqDtos);
        }

    }
}
