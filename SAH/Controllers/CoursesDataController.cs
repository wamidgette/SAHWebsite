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
using System.Diagnostics;
using System.IO;
using System.Web;



namespace SAH.Controllers
{
    public class CoursesDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// A list of Courses in the database
        /// </summary>
        /// <returns>The list of Courses Posted</returns>
        /// <example>
        /// GET: api/CoursesData/GetCourses
        /// </example>
        [ResponseType(typeof(IEnumerable<CoursesDto>))]
        public IHttpActionResult GetCourses()
        {
            //Get list of Course from the database
            List<Courses> Courses = db.Courses.Include(t => t.EmployeeApplicant).ToList();
            

            //Data transfer model with all the information about a Courses
            List<CoursesDto> CoursesDtos = new List<CoursesDto> { };

            //Transfering Courses to data transfer object
            foreach (var Course in Courses)
            {
                CoursesDto NewCourse = new CoursesDto
                {
                    CourseId = Course.CourseId,
                    CourseCode = Course.CourseCode,
                    CourseName = Course.CourseName,
                    StartOn = Course.StartOn,
                    CourseDuration = Course.CourseDuration
                    //NumApplications = Course.EmployeeApplicant.Count()
                };
                CoursesDtos.Add(NewCourse);
            }

            return Ok(CoursesDtos);
        }


    

        /// <summary>
        /// Get a list of Employee Applications linked to the Course
        /// </summary>
        /// <param name="Id">Course Id</param>
        /// <returns>List of Employee Application associated with the Courses</returns>
        /// <example>
        /// GET: api/CoursesData/GetCourseApplications
        /// </example>

        [ResponseType(typeof(IEnumerable<EmployeeApplicantDto>))]
        public IHttpActionResult GetCourseApplications(int id)
        {
            List<EmployeeApplicant> EmployeeApplicants = db.EmployeeApplicant.Where(a => a.CourseId == id)
                .ToList();
            List<EmployeeApplicantDto> EmployeeApplicantDtos = new List<EmployeeApplicantDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var EmployeeApplicant in EmployeeApplicants)
            {
                EmployeeApplicantDto NewEmployeeApplication = new EmployeeApplicantDto
                {
                    EmployeeApplicantId = EmployeeApplicant.EmployeeApplicantId,                    
                    Id = EmployeeApplicant.Id
                };
                EmployeeApplicantDtos.Add(NewEmployeeApplication);
            }

            return Ok(EmployeeApplicantDtos);
        }

        /// <summary>
        /// Find an specific Course from the database
        /// </summary>
        /// <param name="Id">Course Id</param>
        /// <returns>Information about the JCourseob</returns>
        /// <example>
        /// GET: api/CoursesData/FindCourse/5
        /// </example>

        [HttpGet]
        [ResponseType(typeof(CoursesDto))]
        public IHttpActionResult FindCourse(int id)
        {
            Courses Courses = db.Courses.Find(id);
            //if not found, return 404 status code.
            if (Courses == null)
            {
                return NotFound();
            }
            //put into a 'friendly object format'
            CoursesDto CoursesDto = new CoursesDto
            {
                CourseId = Courses.CourseId,
                CourseCode = Courses.CourseCode,
                CourseName = Courses.CourseName,
                StartOn = Courses.StartOn,
                CourseDuration = Courses.CourseDuration
            };

            //pass along data as 200 status code OK response
            return Ok(CoursesDto);

        }

        /// <summary>
        /// Update a Course in the database, The past information is given
        /// </summary>
        /// <param name="Id">Course Id</param>
        /// <param name="Courses">Course Object. Received a POST Data</param>
        /// <returns></returns>
        /// <example>
        /// PUT: api/Courses/5
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IHttpActionResult UpdateCourse(int id, [FromBody] Courses Courses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Courses.CourseId)
            {
                return BadRequest();
            }

            db.Entry(Courses).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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
        /// Add a new Course to the database
        /// </summary>
        /// <param name="Course">Sent a Post form Data</param>
        /// <returns>200 = successful. 404 = not successful</returns>
        /// <example> 
        /// POST: api/CoursesData/AddCourse
        /// </example>

        [ResponseType(typeof(Courses))]
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IHttpActionResult AddCourse([FromBody] Courses Courses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(Courses);
            db.SaveChanges();

            return Ok(Courses.CourseId);
        }

        /// <summary>
        /// Delete a Course from the database
        /// </summary>
        /// <param name="Id">The Id from the Course to delete</param>
        /// <returns>200 = successful. 404 = not successful</returns>
        /// <example>
        /// POST: api/CourseData/DeleteCourse/5
        /// </example>

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IHttpActionResult DeleteCourse(int id)
        {
            Courses Courses = db.Courses.Find(id);
            if (Courses == null)
            {
                return NotFound();
            }

            db.Courses.Remove(Courses);
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
        /// Find a Course in the System
        /// </summary>
        /// <param name="Id">The Course Id</param>
        /// <returns>If the team exists return true</returns>

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}