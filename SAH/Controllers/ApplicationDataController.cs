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
using SAH.Models.ModelViews;

namespace SAH.Controllers
{
    public class ApplicationDataController : ApiController
    {
        //The access to the SAH Project Database
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// Get request from all the Applications in the Database
        /// </summary>
        /// <returns>Application List including the Application Id, Comments, UserId, Job Id</returns>
        /// <example>
        /// GET: api/ApplicationData/GetApplications
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Player Data Controllers
        /// Code was scaffolded and adjusted

        [HttpGet]
        [ResponseType(typeof(IEnumerable<JobDto>))]
        public IHttpActionResult GetApplications()
        {
            //List of Application from databse
            List<Application> Applications = db.Applications.ToList();

            List<ApplicationDto> ApplicationDtos = new List<ApplicationDto> { };

            //Information exposed to the API from the Data Transfer Object about an Application
            foreach (var Application in Applications)
            {
                ApplicationDto NewApplication = new ApplicationDto
                {
                    ApplicationId = Application.ApplicationId,
                    Comment = Application.Comment,
                    Id = Application.Id,
                    JobId = Application.JobId
                };
                ApplicationDtos.Add(NewApplication);
            }

            return Ok(ApplicationDtos);
        }

        /// <summary>
        /// Find an Application by Id from the database
        /// </summary>
        /// <param name="Id">Application Id</param>
        /// <returns>Information from the Application:Application Id, UserId, Comments and JobId</returns>
        /// <returns>Return 200 code response if it is ok, 404 status response If it doesnt find the Application</returns>
        /// <example>
        /// GET: api/ApplicationData/FindApplication/5
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Players Data Controllers
        [HttpGet]
        [ResponseType(typeof(ApplicationDto))]
        public IHttpActionResult FindApplication(int id)
        {

            Application Application = db.Applications.Find(id);
            //If it is not found, The response is a 404 status.
            if (Application == null)
            {
                return NotFound();
            }

            //Info about the Application inside a Data Transfer Object
            ApplicationDto ApplicationDto = new ApplicationDto
            {
                ApplicationId = Application.ApplicationId,
                Comment = Application.Comment,
                Id = Application.Id,
                JobId = Application.JobId
            };

            //Response 200 if it is ok
            return Ok(ApplicationDto);
        }


        /// <summary>
        /// The Application and the user who made the application
        /// </summary>
        /// <param name="id">id from the selected application</param>
        /// <returns>The user linked to an specific Application</returns>
        /// <example>
        /// GET: api/ApplicationData/GetUserForApplication/4
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Players Data Controllers
        [HttpGet]
        [ResponseType(typeof(ApplicationUserDto))]
        public IHttpActionResult GetUserForApplication(int id)
        {

            //Find the User than match the Application
            ApplicationUser ApplicationUser = db.Users
                .Where(u => u.Applications.Any(a => a.ApplicationId == id))
                .FirstOrDefault();

            if (User == null)
            {

                return NotFound();
            }

            //Information selected to be displayed in the API
            ApplicationUserDto ApplicationUserDto = new ApplicationUserDto
            {
                Id = ApplicationUser.Id,
                FirstName = ApplicationUser.FirstName,
                LastName = ApplicationUser.LastName,
                Email = ApplicationUser.Email,
                PhoneNumber = ApplicationUser.PhoneNumber,
                Address = ApplicationUser.Address,
                DateOfBirth = ApplicationUser.DateOfBirth
            };

            return Ok(ApplicationUserDto);
        }

        /// <summary>
        /// Get Application associated with the job
        /// </summary>
        /// <param name="id">Application Id</param>
        /// <returns>Associated Job with an Application</returns>
        /// <example>
        /// GET: Api/ApplicationData/GetJobForApplication/4
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Players Data Controllers

        [ResponseType(typeof(ApplicationUserDto))]
        public IHttpActionResult GetJobForApplication(int id)
        {

            //Job associated with the Application
            Job Job = db.Jobs
                .Where(j => j.Applications.Any(a => a.ApplicationId == id))
                .FirstOrDefault();

            if (Job == null)
            {

                return NotFound();
            }
            //Information selected to be displayed in the API
            JobDto JobDto = new JobDto
            {
                JobId = Job.JobId,
                Position = Job.Position,
                Category = Job.Category,
                Type = Job.Type,
                Requirement = Job.Requirement,
                Deadline = Job.Deadline
            };

            return Ok(JobDto);
        }

        /// <summary>
        /// Get all applications with their linked User and Job
        /// </summary>
        /// <returns>All application and their jobs and users</returns>
        /// <example>
        /// GET: api/applicationdata/getallapplications
        /// </example>

        [ResponseType(typeof(IEnumerable<ShowApplication>))]
        public IHttpActionResult GetAllApplications()
        {

            //List of the Application from the database
            List<Application> Applications = db.Applications.ToList();

            //Using the ShowApplication View Model
            List<ShowApplication> ApplicationDtos = new List<ShowApplication> { };

            foreach (var Application in Applications)
            {
                ShowApplication application = new ShowApplication();

                //Get the user from the OurUsers Database and link them with the application
                ApplicationUser ApplicationUser = db.Users
                    .Where(c => c.Applications
                    .Any(a => a.ApplicationId == Application.ApplicationId))
                    .FirstOrDefault();

                ApplicationUserDto firstUser = new ApplicationUserDto
                {
                    Id = ApplicationUser.Id,
                    FirstName = ApplicationUser.FirstName,
                    LastName = ApplicationUser.LastName,
                    Email = ApplicationUser.Email,
                    PhoneNumber = ApplicationUser.PhoneNumber,
                    Address = ApplicationUser.Address,
                    
                };

                //Get the Job from Jobs Table and associate them with the application
                Job Job = db.Jobs
                    .Where(j => j.Applications
                    .Any(a => a.ApplicationId == Application.ApplicationId))
                    .FirstOrDefault();

                JobDto job = new JobDto
                {
                    Position = Job.Position,
                    Category = Job.Category,
                    Type = Job.Type,
                    Requirement = Job.Requirement,
                    Deadline = Job.Deadline
                };

                ApplicationDto NewApplication = new ApplicationDto
                {
                    ApplicationId = Application.ApplicationId,
                    Comment = Application.Comment,
                    JobId = Application.JobId,
                    Id = Application.Id
                };

                application.Application = NewApplication;
                application.Job = job;
                application.ApplicationUser = firstUser;
                ApplicationDtos.Add(application);
            }

            return Ok(ApplicationDtos);
        }

        /// <summary>
        /// Update an Application in the database, The past information is given
        /// </summary>
        /// <param name="id">Application Id</param>
        /// <param name="Application">Received a POST Data</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/Applicationdata/UpdateApplication/5
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Players Data Controllers

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateApplication(int id, [FromBody] Application Application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Application.ApplicationId)
            {
                return BadRequest();
            }

            db.Entry(Application).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
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
        /// Add Application to the database
        /// </summary>
        /// <param name="Application">Application Id, Sent a Post Form Data</param>
        /// <returns>Add a new Application, return a 200 response if it is ok and 404 if it fails</returns>
        // <example>
        ///POST: api/Application/AddApplication
        ///</example>
        /// Reference: Varsity Project by Christine Bittle - Players Data Controllers
        [HttpPost]
        [ResponseType(typeof(Application))]
        public IHttpActionResult AddApplication([FromBody] Application Application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Applications.Add(Application);
            db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = Application.ApplicationId }, Application);

            return Ok(Application.ApplicationId);
        }

        /// <summary>
        /// Delete an application by Id
        /// </summary>
        /// <param name="id">Id from the application to delete</param>
        /// <returns>
        /// Delete an application by Id        
        /// </returns>       
        /// <example>
        /// Post: api/Applicationdata/deleteapplication/5
        /// </example>
        /// Reference: Varsity Project by Christine Bittle - Players Data Controllers
        [HttpPost]
        [ResponseType(typeof(Application))]
        public IHttpActionResult DeleteApplication(int id)
        {
            Application Application = db.Applications.Find(id);
            if (Application == null)
            {
                return NotFound();
            }

            db.Applications.Remove(Application);
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
        /// Finds an Application
        /// </summary>
        /// <param name="id">The Application id</param>
        /// <returns>Application exist = True, Application does not exist False</returns>
        private bool ApplicationExists(int id)
        {
            return db.Applications.Count(a => a.ApplicationId == id) > 0;
        }
    }
}