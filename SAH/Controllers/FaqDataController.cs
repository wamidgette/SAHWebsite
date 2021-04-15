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
        /// GET: api/faqData/Getfaqs
        /// </example>

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
                    DepartmentID = faq.DepartmentID
                };
                faqDtos.Add(newFaq);
            }

            return Ok(faqDtos);
        }


        [ResponseType(typeof(IEnumerable<FaqDto>))]
        public IHttpActionResult GetFaq(int id)
        {
            List<Faq> Faqs = db.Faqs.Where(f => f.FaqID == id)
                .ToList();
            List<FaqDto> FaqDtos = new List<FaqDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Faq in Faqs)
            {
                FaqDto NewFaq = new FaqDto
                {
                    FaqID = Faq.FaqID,
                    Question = Faq.Question,
                    Answer = Faq.Answer,
                    Publish = Faq.Publish,
                    DepartmentID =Faq.DepartmentID
                };
                FaqDtos.Add(NewFaq);
            }

            return Ok(FaqDtos);
        }



        /// <summary>
        /// Update a Faq in the database, The past information is given
        /// </summary>
        /// <param name="Id">Job Id</param>
        /// <param name="Job">Job Object. Received a POST Data</param>
        /// <returns></returns>
        /// <example>
        /// PUT: api/Jobs/5
        /// </example>
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateJob(int id, [FromBody] Job Job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Job.JobId)
            {
                return BadRequest();
            }

            db.Entry(Job).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
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
        /// Add a new Job to the database
        /// </summary>
        /// <param name="Job">Sent a Post form Data</param>
        /// <returns>200 = successful. 404 = not successful</returns>
        /// <example> 
        /// POST: api/JobData/AddJob
        /// </example>

        [ResponseType(typeof(Job))]
        public IHttpActionResult AddJob([FromBody] Job Job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Jobs.Add(Job);
            db.SaveChanges();

            return Ok(Job.JobId);
        }

        /// <summary>
        /// Delete a Job from the database
        /// </summary>
        /// <param name="Id">The Id from the Job to delete</param>
        /// <returns>200 = successful. 404 = not successful</returns>
        /// <example>
        /// POST: api/JobData/DeleteJob/5
        /// </example>

        [ResponseType(typeof(Job))]
        public IHttpActionResult DeleteJob(int id)
        {
            Job Job = db.Jobs.Find(id);
            if (Job == null)
            {
                return NotFound();
            }

            db.Jobs.Remove(Job);
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
        /// Find a Job in the System
        /// </summary>
        /// <param name="Id">The Job Id</param>
        /// <returns>If the team exists return true</returns>

        private bool JobExists(int id)
        {
            return db.Jobs.Count(e => e.JobId == id) > 0;
        }
    }
}
    