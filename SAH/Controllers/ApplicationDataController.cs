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

        // GET: api/Applications
        [ResponseType(typeof(IEnumerable<JobDto>))]
        public IHttpActionResult GetApplications()
        {
            //Getting the list of Application objects from the databse
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

        // GET: api/Applications/5
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
            ApplicationDto ApplicationDto = new ApplicationDto
            {
                ApplicationId = Application.ApplicationId,
                Comment = Application.Comment,                
                UserId = Application.UserId,
                JobId = Application.JobId
            };

            //pass along data as 200 status code OK response
            return Ok(ApplicationDto);
        }

        [ResponseType(typeof(IEnumerable<UserDto>))]
        public IHttpActionResult GetUsers()
        {
            //List of all users who potentially use the parking
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















        // PUT: api/Applications/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutApplication(int id, Application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != application.ApplicationId)
            {
                return BadRequest();
            }

            db.Entry(application).State = EntityState.Modified;

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

        // POST: api/Applications
        [ResponseType(typeof(Application))]
        public IHttpActionResult PostApplication(Application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Applications.Add(application);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = application.ApplicationId }, application);
        }

        // DELETE: api/Applications/5
        [ResponseType(typeof(Application))]
        public IHttpActionResult DeleteApplication(int id)
        {
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return NotFound();
            }

            db.Applications.Remove(application);
            db.SaveChanges();

            return Ok(application);
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