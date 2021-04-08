using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SAH.Models;
using System.Diagnostics;

namespace SAH.Controllers
{
    public class JobDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// A list of Job in the database
        /// </summary>
        /// <returns>The list of Jobs Posted</returns>
        /// <example>
        /// GET: api/JobData/GetJobs
        /// </example>
        [ResponseType(typeof(IEnumerable<JobDto>))]
        public IHttpActionResult GetJobs()
        {
            //Get list of Job from the database
            List<Job> Jobs = db.Jobs.ToList();

            //Data transfer model with all the information about a Job
            List<JobDto> JobDtos = new List<JobDto> { };

            //Transfering Job to data transfer object
            foreach (var Job in Jobs)
            {
                JobDto NewJob = new JobDto
                {
                    JobId = Job.JobId,
                    Position = Job.Position,
                    Category = Job.Category,
                    Type = Job.Type,
                    Requirement = Job.Requirement,
                    Deadline = Job.Deadline

                };
                JobDtos.Add(NewJob);
            }

            return Ok(JobDtos);
        }

        /// <summary>
        /// Get a list of Applications linked to the Job
        /// </summary>
        /// <param name="Id">Job Id</param>
        /// <returns>List of Application associated with the Job</returns>
        /// <example>
        /// GET: api/JobData/GetJobApplications
        /// </example>

        [ResponseType(typeof(IEnumerable<ApplicationDto>))]
        public IHttpActionResult GetJobApplications(int Id)
        {
            List<Application> Applications = db.Applications.Where(a => a.JobId == Id)
                .ToList();
            List<ApplicationDto> ApplicationDtos = new List<ApplicationDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Application in Applications)
            {
                ApplicationDto NewApplication = new ApplicationDto
                {
                    ApplicationId = Application.ApplicationId,
                    Comment = Application.Comment,
                    JobId = Application.JobId,
                    UserId = Application.UserId
                };
                ApplicationDtos.Add(NewApplication);
            }

            return Ok(ApplicationDtos);
        }

        /// <summary>
        /// Find an specific Job from the database
        /// </summary>
        /// <param name="Id">Job Id</param>
        /// <returns>Information about the Job</returns>
        /// <example>
        /// GET: api/JobData/FindJob/5
        /// </example>

        [HttpGet]
        [ResponseType(typeof(JobDto))]
        public IHttpActionResult FindJob(int Id)
        {
            Job Job = db.Jobs.Find(Id);
            //if not found, return 404 status code.
            if (Job == null)
            {
                return NotFound();
            }
            //put into a 'friendly object format'
            JobDto JobDto = new JobDto
            {
                JobId = Job.JobId,
                Position = Job.Position,
                Category = Job.Category,
                Type = Job.Type,
                Requirement = Job.Requirement,
                Deadline = Job.Deadline
            };

            //pass along data as 200 status code OK response
            return Ok(JobDto);

        }

        /// <summary>
        /// Update a Job in the database, The past information is given
        /// </summary>
        /// <param name="Id">Job Id</param>
        /// <param name="Job">Job Object. Received a POST Data</param>
        /// <returns></returns>
        /// <example>
        /// PUT: api/Jobs/5
        /// </example>

        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateJob(int Id, [FromBody] Job Job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id != Job.JobId)
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
                if (!JobExists(Id))
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
        public IHttpActionResult DeleteJob(int Id)
        {
            Job Job = db.Jobs.Find(Id);
            if (Job == null)
            {
                return NotFound();
            }

            db.Jobs.Remove(Job);
            db.SaveChanges();

            return Ok(Job);
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

        private bool JobExists(int Id)
        {
            return db.Jobs.Count(e => e.JobId == Id) > 0;
        }
    }
}