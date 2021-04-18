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
using System.IO;
using System.Web;


namespace SAH.Controllers
{
    public class JobDataController : ApiController
    {
        //The access to the SAH Project Database
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// Get request from all the Jobs in the Database
        /// </summary>
        /// <returns>Job' List including the Job Id, Position, Category, Type, Requirement and Deadline</returns>
        /// <example>
        /// GET: api/JobData/GetJobs
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers
        /// Code was scaffolded and adjusted
        
        [ResponseType(typeof(IEnumerable<JobDto>))]
        public IHttpActionResult GetJobs()
        {
            //List of Job from the database
            List<Job> Jobs = db.Jobs.ToList();
            
            List<JobDto> JobDtos = new List<JobDto> { };
            //Information exposed to the API from the Data Transfer Object about a Job
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
        /// Get a list of Applications linked to the Job.
        /// </summary>
        /// <param name="Id">Job Id</param>
        /// <returns>List of Application associated with the Job</returns>
        /// <example>
        /// GET: api/JobData/GetJobApplicationsForJob
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers

        [ResponseType(typeof(IEnumerable<ApplicationDto>))]
        public IHttpActionResult GetApplicationsForJob(int id)
        {   
            //List of all application by Job
            List<Application> Applications = db.Applications
                .Where(a => a.JobId == id)
                .ToList();
            List<ApplicationDto> ApplicationDtos = new List<ApplicationDto> { };

            //Information exposed to the API from the Data Transfer Object about a Job
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
        /// Find a Job by Id from the database
        /// </summary>
        /// <param name="Id">Job Id</param>
        /// <returns>Information from the Job:Position, Category, Type, Requirement and Deadline</returns>
        /// <returns>Return 200 code response if it is ok, 404 status response If it doesnt find the Job</returns>
        /// <example>
        /// GET: api/JobData/FindJob/5
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers

        [HttpGet]
        [ResponseType(typeof(JobDto))]
        public IHttpActionResult FindJob(int id)
        {
            Job Job = db.Jobs.Find(id);
            //It will look for the Job in the database if not it will return 404 status
            if (Job == null)
            {
                return NotFound();
            }
            
            JobDto JobDto = new JobDto
            {
                JobId = Job.JobId,
                Position = Job.Position,
                Category = Job.Category,
                Type = Job.Type,
                Requirement = Job.Requirement,
                Deadline = Job.Deadline
            };

            //Return a 200 status code if everything is OK
            return Ok(JobDto);

        }
        /// <summary>
        /// Update a Job in the database, showing the information already in the system from the player
        /// </summary>
        /// <param name="Id">Job Id</param>
        /// <param name="Job">Post Data is received</param>
        /// <example>
        /// Post: api/JobData/UpdateJob/5
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers
        
        [HttpPost]
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
        /// <returns>Return 200 code response if it is Ok or 404 response code if it is not ok!</returns>
        /// <example> 
        /// POST: api/JobData/AddJob
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers

        [ResponseType(typeof(Job))]
        [HttpPost]
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
        /// <returns>Return 200 code response if it is Ok or 404 response code if it is not ok!</returns>
        /// <example>
        /// POST: api/JobData/DeleteJob/5
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers
        [HttpPost]
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
        /// <returns>If the job exists return true</returns>

        private bool JobExists(int id)
        {
            return db.Jobs.Count(j => j.JobId == id) > 0;
        }
    }
}