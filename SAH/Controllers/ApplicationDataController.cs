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
        private SAHDataContext db = new SAHDataContext();


        /// <summary>
        /// List of all Ticket from the database
        /// </summary>
        /// <returns>The list of Tickets</returns>
        /// <example>
        /// GET: api/Application/GetApplication
        /// </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ApplicationDto>))]
        public IHttpActionResult GetApplications()
        {
            //List of Application from databse
            List<Application> Applications = db.Applications.ToList();
            List<ApplicationDto> ApplicationDtos = new List<ApplicationDto> { };

            //Transfering Application to data transfer object
            foreach (var Application in Applications)
            {
                ApplicationDto NewApplication = new ApplicationDto
                {
                    ApplicationId = Application.ApplicationId,
                    Comment = Application.Comment,
                    UserId = Application.UserId,
                    JobId = Application.JobId
                };
                ApplicationDtos.Add(NewApplication);
            }

            return Ok(ApplicationDtos);
        }


        /// <summary>
        /// Find an specific Application in the database
        /// </summary>
        /// <param name="id">Job Id</param>
        /// <returns>Information abut the Application</returns>
        /// <example>
        /// GET: api/ApplicationData/FindApplication/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ApplicationDto))]
        public IHttpActionResult FindApplication(int id)
        {
            //Find the data
            Application Application = db.Applications.Find(id);
            //if not found, return 404 status code.
            if (Application == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            ApplicationDto Application1 = new ApplicationDto
            {
                ApplicationId = Application.ApplicationId,
                Comment = Application.Comment,
                UserId = Application.UserId,
                JobId = Application.JobId
            };

            //pass along data as 200 status code OK response
            return Ok(Application1);
        }

        /// <summary>
        /// Get all users in the database
        /// </summary>
        /// <returns>ALl users in the database</returns>
        /// <example>
        /// GET: api/ApplicationData/GetUsers
        /// </example>

        [ResponseType(typeof(IEnumerable<UserDto>))]
        public IHttpActionResult GetUsers()
        {
            //List of all users
            List<User> Users = db.OurUsers.ToList();
            List<UserDto> UserDtos = new List<UserDto> { };

            foreach (var User in Users)
            {
                UserDto NewUser = new UserDto
                {
                    UserId = User.UserId,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Email = User.Email,
                    Phone = User.Phone,
                    Address = User.Address,
                    DateOfBirth = User.DateOfBirth

                };
                UserDtos.Add(NewUser);
            }

            return Ok(UserDtos);
        }

        /// <summary>
        /// Get all Jobs in the Database
        /// </summary>
        /// <returns>List Job Positions</returns>
        /// <example>
        /// GET: api/applicationdata/getjobs
        /// </example>
        [ResponseType(typeof(IEnumerable<JobDto>))]
        public IHttpActionResult GetJobs()
        {
            //Getting the list of Jobs
            List<Job> Jobs = db.Jobs.ToList();


            List<JobDto> JobDtos = new List<JobDto> { };


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
        /// The Application and the user who made the application
        /// </summary>
        /// <param name="id">id of the selected application</param>
        /// <returns></returns>
        /// <example>
        /// GET: api/applicationdata/getapplicationuser/4
        /// </example>
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetApplicationUser(int id)
        {

            //Associate the job Application with user
            User User = db.OurUsers.Where(c => c.Applications.Any(a => a.ApplicationId == id)).FirstOrDefault();


            if (User == null)
            {

                return NotFound();
            }

            //Here you can choose which information is exposed to the API
            UserDto OwnerUser = new UserDto
            {
                UserId = User.UserId,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                Phone = User.Phone,
                Address = User.Address,
                DateOfBirth = User.DateOfBirth
            };

            return Ok(OwnerUser);
        }


        /// <summary>
        /// Get Application associated with the job
        /// </summary>
        /// <param name="id">Application Id</param>
        /// <returns>Application and associated jobs </returns>
        /// <example>
        /// GET: Api/applicationdata/getapplicationjob/4
        /// </example>
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetApplicationJob(int id)
        {

            //Application associated with the user and job
            Job Job = db.Jobs.Where(j => j.Applications.Any(a => a.ApplicationId == id)).FirstOrDefault();

            if (Job == null)
            {

                return NotFound();
            }

            JobDto JobFirst = new JobDto
            {
                JobId = Job.JobId,
                Position = Job.Position,
                Category = Job.Category,
                Type = Job.Type,
                Requirement = Job.Requirement,
                Deadline = Job.Deadline
            };

            return Ok(JobFirst);
        }

        /// <summary>
        /// Get all application, with the user and the job conencted to
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// GET: api/applicationdata/getallapplications/5
        /// </example>
        [ResponseType(typeof(IEnumerable<ShowApplication>))]
        public IHttpActionResult GetAllApplications()
        {

            //List of the tickets from the database
            List<Application> Applications = db.Applications.ToList();

            //Data transfer object to show information about the ticket
            List<ShowApplication> ApplicationDtos = new List<ShowApplication> { };

            foreach (var Application in Applications)
            {
                ShowApplication application = new ShowApplication();

                //Get the user to which the ticket belongs to
                User User = db.OurUsers.Where(c => c.Applications.Any(a => a.ApplicationId == Application.ApplicationId)).FirstOrDefault();

                UserDto parentUser = new UserDto
                {
                    UserId = User.UserId,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Email = User.Email,
                    Phone = User.Phone,
                    Address = User.Address,
                    DateOfBirth = User.DateOfBirth
                };
                //Get the parking spot of ticket
                Job Job = db.Jobs.Where(j => j.Applications.Any(a => a.ApplicationId == Application.ApplicationId)).FirstOrDefault();

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
                    UserId = Application.UserId
                };

                application.Application = NewApplication;
                application.Job = job;
                application.User = parentUser;
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
        // POST: api/Applicationdata/updateapplication/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateApplication(int id, Application Application)
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
        /// <returns>Add a new Application</returns>
        // <example>
        ///POST: api/Applications
        ///</example>
        [HttpPost]
        [ResponseType(typeof(Application))]
        public IHttpActionResult AddApplication(Application Application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Applications.Add(Application);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Application.ApplicationId }, Application);
        }

        /// <summary>
        /// Delete an application by Id
        /// </summary>
        /// <param name="id">application Id to the aplication to delete</param>
        /// <returns>
        /// Delete an application by Id
        /// 200 = successful. 404 = not successful
        /// </returns>        /// 
        // DELETE: api/Applicationdata/deleteapplication/5
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

            return Ok(Application);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationExists(int id)
        {
            return db.Applications.Count(e => e.ApplicationId == id) > 0;
        }
    }
}