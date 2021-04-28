﻿using System;
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
using Microsoft.AspNet.Identity;

namespace SAH.Controllers
{
    public class EmployeeApplicantsDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();


        /// <summary>
        /// List of all Employee Applications from the database
        /// </summary>
        /// <returns>The list of Employee Applications</returns>
        /// <example>
        /// GET: api/EmployeeApplicantsData/GetAllApplications
        /// </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CoursesDto>))]
        public IHttpActionResult GetEmployeeApplicant()
        {
            //Getting the list of application  objects from the databse
            List<EmployeeApplicant> EmployeeApplicants = db.EmployeeApplicant.ToList();
            List<EmployeeApplicantDto> EmployeeApplicantDtos = new List<EmployeeApplicantDto> { };

            //Transfering application to data transfer object
            foreach (var EmployeeApplicant in EmployeeApplicants)
            {
                EmployeeApplicantDto NewEmployeeApplicant = new EmployeeApplicantDto
                {
                    EmployeeApplicantId = EmployeeApplicant.EmployeeApplicantId,
                    Reason = EmployeeApplicant.Reason,
                    Id = EmployeeApplicant.Id,
                    CourseId = EmployeeApplicant.CourseId
                };
                EmployeeApplicantDtos.Add(NewEmployeeApplicant);
            }

            return Ok(EmployeeApplicantDtos);
        }



        /// <summary>
        /// Find an specific Employee Application in the database
        /// </summary>
        /// <param name="id">EmployeeAppicantId</param>
        /// <returns>Information abut the Employee Application</returns>
        /// <example>
        /// GET: api/EmployeeApplicantsData/FindApplication/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(EmployeeApplicantDto))]
        public IHttpActionResult FindApplication(int id)
        {
            EmployeeApplicant EmployeeApplicant = db.EmployeeApplicant.Find(id);

            if (EmployeeApplicant == null)
            {
                return NotFound();
            }

            //A data transfer object model used to show only most relevant information
            EmployeeApplicantDto TempEmployeeApplicant = new EmployeeApplicantDto
            {
                EmployeeApplicantId = EmployeeApplicant.EmployeeApplicantId,
                Reason = EmployeeApplicant.Reason,
                Id = EmployeeApplicant.Id,
                CourseId = EmployeeApplicant.CourseId
            };

            return Ok(TempEmployeeApplicant);

        }

        /// <summary>
        /// This method gets all the users from the table
        /// <example>GET: api/EmployeeApplicantsData/GetUsers</example>
        /// </summary>
        /// <returns>The list of all users</returns>

        [ResponseType(typeof(IEnumerable<ApplicationUserDto>))]
        public IHttpActionResult GetUsers()
        {
            //List of all users who can apply for course
            List<ApplicationUser> Users = db.Users.ToList();
            List<ApplicationUserDto> UserDtos = new List<ApplicationUserDto> { };

            foreach (var User in Users)
            {
                ApplicationUserDto NewUser = new ApplicationUserDto
                {
                    Id = User.Id,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    EmployeeNumber = User.EmployeeNumber,
                    Email = User.Email
                };
                UserDtos.Add(NewUser);
            }

            return Ok(UserDtos);
        }

        /// <summary>
        /// This method gets the list of Courses from the database
        /// <example> GET: api/EmployeeApplicantsData/GetApplicationCourse/ </example>
        /// </summary>
        /// <returns> The list of Courses from the database</returns>

        [ResponseType(typeof(IEnumerable<CoursesDto>))]
        public IHttpActionResult GetCourses()
        {
            //Getting the list of Courses objects from the databse
            List<Courses> Courses = db.Courses.ToList();

            //Here a data transfer model is used to keep only the information to be displayed about a course
            List<CoursesDto> CoursesDtos = new List<CoursesDto> { };

            //Transfering course to data transfer object
            foreach (var Course in Courses)
            {
                CoursesDto NewCourse = new CoursesDto
                {
                    CourseId = Course.CourseId,
                    CourseCode = Course.CourseCode,
                    CourseName = Course.CourseName
                };
                CoursesDtos.Add(NewCourse);
            }

            return Ok(CoursesDtos);
        }

        /// <summary>
        /// This method provides the user to which the current application belongs
        /// <example>api/EmployeeApplicantsData/GetApplicationUser/2</example>
        /// </summary>
        /// <param name="id">ID of the selected Employee Application</param>
        /// <returns>The user to which current application belongs</returns>


        [ResponseType(typeof(ApplicationUserDto))]
        public IHttpActionResult GetApplicationUser(int id)
        {

            //Find the user to which the current application belongs
            ApplicationUser user = db.Users.Where(c => c.EmployeeApplicants.Any(p => p.EmployeeApplicantId == id)).FirstOrDefault();

            //In case this user does not exist
            if (user == null)
            {

                return NotFound();
            }

            ApplicationUserDto OwnerUser = new ApplicationUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmployeeNumber = user.EmployeeNumber,
                Email = user.Email
            };

            return Ok(OwnerUser);
        }
     
        /// <summary>
        /// This method provides the course to which the current application belongs
        /// <example>api/EmployeeApplicantsData/GetCourses/1</example>
        /// </summary>
        /// <param name="id">ID of the selected application</param>
        /// <returns>The course to which current application belongs</returns>

        [ResponseType(typeof(CoursesDto))]
        public IHttpActionResult GetCourseForApplication(int id)
        {

            //Find the course which is selected in the current application
            Courses Courses = db.Courses.Where(c => c.EmployeeApplicant.Any(p => p.EmployeeApplicantId == id)).FirstOrDefault();

            //In case this user does not exist
            if (Courses == null)
            {

                return NotFound();
            }

            CoursesDto Course = new CoursesDto
            {
                CourseId = Courses.CourseId,
                CourseCode = Courses.CourseCode,
                CourseName = Courses.CourseName
            };

            return Ok(Courses);
        }

        /// <summary>
        /// Returns a list of applications for a given user.
        /// </summary>
        /// <param name="id">The input UserID (string)</param>
        /// <returns>A list of applications applied by that user</returns>
        /// <example>
        /// GET api/EmployeeApplicantData/GetApplicationsForUser/abcedf-12345-ghijkl
        /// </example>
        public IHttpActionResult GetApplicationsForUser(string id)
        {
            IEnumerable<EmployeeApplicant> EmployeeApplicantId = db.EmployeeApplicant.Where(s => s.Id == id);
            List<EmployeeApplicantDto> EmployeeApplicationsDtos = new List<EmployeeApplicantDto>() { };

            foreach (var applications in EmployeeApplicantId)
            {
                EmployeeApplicantDto AppliedcoursesDto = new EmployeeApplicantDto
                {
                    EmployeeApplicantId = applications.EmployeeApplicantId,
                    Reason = applications.Reason,
                    CourseId = applications.CourseId,
                    Id = applications.Id
                };
                EmployeeApplicationsDtos.Add(AppliedcoursesDto);
            }

            return Ok(EmployeeApplicationsDtos);
        }

        /// <summary>
        /// This method permits to update the selected application
        /// <example>api/EmployeeApplicantsData/UpdateApplication/1</example>
        /// </summary>
        /// <param name="id">The ID of the application</param>
        /// <param name="EmployeeApplicant">The current application itself</param>
        /// <returns>Saves the current application with new values to the database</returns>

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateApplication(int id, [FromBody] EmployeeApplicant EmployeeApplicant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != EmployeeApplicant.EmployeeApplicantId)
            {
                return BadRequest();
            }

            db.Entry(EmployeeApplicant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeApplicantExists(id))
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
        /// This method permits to add a new application to the database
        /// <example>POST: api/EmployeeApplicantsData/AddApplication</example>
        /// </summary>
        /// <param name="EmployeeApplicant">The actual application to be added</param>
        /// <returns> It adds a new Employee Application to the database</returns>

        [HttpPost]
        [ResponseType(typeof(EmployeeApplicant))]
        public IHttpActionResult AddApplication(EmployeeApplicant EmployeeApplicant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeApplicant.Add(EmployeeApplicant);
            db.SaveChanges();

            return Ok(EmployeeApplicant.EmployeeApplicantId);
            //return CreatedAtRoute("DefaultApi", new { id = EmployeeApplicant.EmployeeApplicantId }, EmployeeApplicant);
        }

        /// <summary>
        /// This method gets the list of all applications from the database with their user and course information
        /// <example> GET: api/EmployeeApplicantsData/GetAllApplications</example>
        /// </summary>
        /// <returns> The list of employee applications, the courses and user to which they belong to from the database</returns>

        [ResponseType(typeof(IEnumerable<ShowEmployeeApplicant>))]
        public IHttpActionResult GetAllApplications()
        {

            //List of the applications from the database
            List<EmployeeApplicant> EmployeeApplicants = db.EmployeeApplicant.ToList();

            //Data transfer object to show information about the application
            List<ShowEmployeeApplicant> EmployeeApplicantDtos = new List<ShowEmployeeApplicant> { };

            foreach (var EmployeeApplicant in EmployeeApplicants)
            {
                ShowEmployeeApplicant EmployeeApplication = new ShowEmployeeApplicant();

                //Get the user to which the application belongs to
                ApplicationUser user = db.Users.Where(c => c.EmployeeApplicants.Any(m => m.EmployeeApplicantId == EmployeeApplicant.EmployeeApplicantId)).FirstOrDefault();

                ApplicationUserDto parentUser = new ApplicationUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,  
                    EmployeeNumber = user.EmployeeNumber,
                    Email = user.Email

                };
                //Get the course of application
                Courses Course = db.Courses.Where(l => l.EmployeeApplicant.Any(m => m.EmployeeApplicantId == EmployeeApplicant.EmployeeApplicantId)).FirstOrDefault();

          
                CoursesDto Courses = new CoursesDto
                {
                    CourseId = Course.CourseId,
                    CourseCode = Course.CourseCode,
                    CourseName = Course.CourseName
                };


                EmployeeApplicantDto NewEmployeeApplicant = new EmployeeApplicantDto
                {
                    EmployeeApplicantId = EmployeeApplicant.EmployeeApplicantId,
                    Id = EmployeeApplicant.Id,
                    CourseId = EmployeeApplicant.CourseId

                };



                EmployeeApplication.EmployeeApplicant = NewEmployeeApplicant;
                EmployeeApplication.Courses = Courses;
                EmployeeApplication.User = parentUser;
                EmployeeApplicantDtos.Add(EmployeeApplication);
            }

            return Ok(EmployeeApplicantDtos);
        }

        /// <summary>
        /// This method deletes the Employee Application with the provided ID 
        /// <example>api/EmployeeApplicantsData/DeleteApplication/1</example>
        /// </summary>
        /// <param name="id">ID of the employee application</param>
        /// <returns>Deletes the employee aapication from the database</returns>

        [HttpPost]
        [ResponseType(typeof(EmployeeApplicant))]
                public IHttpActionResult DeleteApplication(int id)
        {
            EmployeeApplicant EmployeeApplicant = db.EmployeeApplicant.Find(id);
            if (EmployeeApplicant == null)
            {
                return NotFound();
            }

            db.EmployeeApplicant.Remove(EmployeeApplicant);
            db.SaveChanges();

            return Ok(EmployeeApplicant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeApplicantExists(int id)
        {
            return db.EmployeeApplicant.Count(e => e.EmployeeApplicantId == id) > 0;
        }
    }
}
