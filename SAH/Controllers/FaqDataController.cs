using SAH.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace SAH.Controllers
{
    public class FaqDataController : ApiController
    {
        //This variable is our database access point
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// A list of FAQ in the database
        /// </summary>
        /// <returns>The list of Faqs Posted</returns>
        /// <example>
        /// GET: api/FaqData/GetFaqs
        /// </example>
        [HttpGet]
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
                    Answer = faq.Answer,
                    Publish = faq.Publish,
                    DepartmentID = faq.DepartmentID,
                    DepartmentName = faq.Department.DepartmentName
                };
                faqDtos.Add(newFaq);
            }

            return Ok(faqDtos);
        }

        /// <summary>
        /// A list of Published FAQs
        /// </summary>
        /// <returns>The list of Faqs Posted</returns>
        /// <example>
        /// GET: api/FaqData/GetPublicFaqs
        /// </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<FaqDto>))]
        public IHttpActionResult GetPublicFaqs()
        {
            List<Faq> faqs = db.Faqs.Where(f=>f.Publish).ToList();
            List<FaqDto> faqDtos = new List<FaqDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var faq in faqs)
            {
                FaqDto newFaq = new FaqDto
                {
                    FaqID = faq.FaqID,
                    Question = faq.Question,
                    Answer = faq.Answer,
                    Publish = faq.Publish,
                    DepartmentID = faq.DepartmentID,
                    DepartmentName = faq.Department.DepartmentName
                };
                faqDtos.Add(newFaq);
            }

            return Ok(faqDtos);
        }



        /// <summary>
        /// Finds a particular FAQ in the database with a 200 status code. If the Sponsor is not found, return 404.
        /// </summary>
        /// <param name="id">The FAQ id</param>
        /// <returns>Information about the FAQ</returns>
        // <example>
        // GET: api/FaqData/FindFaq/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<FaqDto>))]
        public IHttpActionResult FindFaq(int id)
        {
            Faq faq = db.Faqs.Find(id);

            FaqDto faqDto = new FaqDto
            {
                FaqID = faq.FaqID,
                Question = faq.Question,
                Answer = faq.Answer,
                Publish = faq.Publish,
                DepartmentID = faq.DepartmentID
            };

            return Ok(faqDto);
        }

        /// <summary>
        /// Update a Faq in the database, The past information is given
        /// </summary>
        /// <param name="Id">FAQ Id</param>
        /// <param name="Faq">FAQ Object. Received a POST Data</param>
        /// <returns></returns>
        /// <example>
        /// PUT: api/faqdata/updatefaq/5
        /// </example>   
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateFaq(int id, [FromBody] Faq faq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != faq.FaqID)
            {
                return BadRequest();
            }

            db.Entry(faq).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaqExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add a new FAQ to the database
        /// </summary>
        /// <param name="FAQ">Sent a Post form Data</param>
        /// <returns>200 = successful. 404 = not successful</returns>
        /// <example> 
        /// POST: api/FaqData/AddFaq
        /// </example>

        [ResponseType(typeof(Faq))]
        public IHttpActionResult AddFaq([FromBody] Faq faq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Faqs.Add(faq);
            db.SaveChanges();

            return Ok(faq.FaqID);
        }

        /// <summary>
        /// Delete a FAQ from the database
        /// </summary>
        /// <param name="Id">The Id from the FAQ to delete</param>
        /// <returns>200 = successful. 404 = not successful</returns>
        /// <example>
        /// POST: api/FAqData/DeleteFaq/2
        /// </example>

        [ResponseType(typeof(Faq))]
        public IHttpActionResult DeleteFaq(int id)
        {
            Faq faq = db.Faqs.Find(id);
            if (faq == null)
            {
                return NotFound();
            }

            db.Faqs.Remove(faq);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Find a Faq in the System
        /// </summary>
        /// <param name="Id">The Faq Id</param>
        /// <returns>If the team exists return true</returns>

        private bool FaqExists(int id)
        {
            return db.Faqs.Any(e => e.FaqID == id);
        }
    }
}
    