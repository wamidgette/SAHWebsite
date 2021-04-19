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
    public class EmployeeApplicantsDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();


        /// <summary>
        /// List of all Application from the database
        /// </summary>
        /// <returns>The list of Applications</returns>
        /// <example>
        /// GET: api/Application/GetApplication
        /// </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CoursesDto>))]
        public IHttpActionResult GetEmployeeApplicant()
        {
            //Getting the list of tickets  objects from the databse
            List<EmployeeApplicant> EmployeeApplicants = db.EmployeeApplicant.ToList();
            List<EmployeeApplicantDto> EmployeeApplicantDtos = new List<EmployeeApplicantDto> { };

            //Transfering Ticket to data transfer object
            foreach (var EmployeeApplicant in EmployeeApplicants)
            {
                EmployeeApplicantDto NewEmployeeApplicant = new EmployeeApplicantDto
                {
                    EmployeeApplicantId = EmployeeApplicant.EmployeeApplicantId,
                    UserId = EmployeeApplicant.UserId,
                    CourseId = EmployeeApplicant.CourseId
                };
                EmployeeApplicantDtos.Add(NewEmployeeApplicant);
            }

            return Ok(EmployeeApplicantDtos);
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
        [ResponseType(typeof(EmployeeApplicantDto))]
        public IHttpActionResult FindApplication(int id)
        {
            EmployeeApplicant EmployeeApplicant = db.EmployeeApplicant.Find(id);

            if (EmployeeApplicant == null)
            {
                return NotFound();
            }

            //A data transfer object model used to show only most important information about the ticket
            EmployeeApplicantDto TempEmployeeApplicant = new EmployeeApplicantDto
            {
                EmployeeApplicantId = EmployeeApplicant.EmployeeApplicantId,
                UserId = EmployeeApplicant.UserId,
                CourseId = EmployeeApplicant.CourseId
            };

            return Ok(TempEmployeeApplicant);

        }

        /// <summary>
        /// This method gets all the users from the table
        /// <example>GET: api/TicketData/GetUsers</example>
        /// <example>GET: api/TicketData/GetUsers</example>
        /// </summary>
        /// <returns>The list of all users</returns>

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
                    RoleId = User.RoleId
                };
                UserDtos.Add(NewUser);
            }

            return Ok(UserDtos);
        }

        /// <summary>
        /// This method gets from the database the list of the all parking spots
        /// <example> GET: api/TicketData/GetParkingSpots </example>
        /// </summary>
        /// <returns> The list of parking spots from the database</returns>

        [ResponseType(typeof(IEnumerable<CoursesDto>))]
        public IHttpActionResult GetCourses()
        {
            //Getting the list of parking spots  objects from the databse
            List<Courses> Courses = db.Courses.ToList();

            //Here a data transfer model is used to keep only the information to be displayed about a parking spot object
            List<CoursesDto> CoursesDtos = new List<CoursesDto> { };

            //Transfering parking spot to data transfer object
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
        /// This method provides the user to which the current ticket belongs
        /// <example>api/TicketData/GetTicketUser/1</example>
        /// <example>api/TicketData/GetTicketUser/3</example>
        /// </summary>
        /// <param name="id">ID of the selected ticket</param>
        /// <returns>The user to which current ticket belongs</returns>

       
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetApplicationUser(int id)
        {

            //Find the owner/user to which the current ticket belongs
            User user = db.OurUsers.Where(c => c.EmployeeApplicants.Any(p => p.EmployeeApplicantId == id)).FirstOrDefault();

            //In case this user does not exist
            if (user == null)
            {

                return NotFound();
            }

            UserDto OwnerUser = new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = user.RoleId
            };

            return Ok(OwnerUser);
        }

        /// <summary>
        /// This method provides the parking spot to which the current ticket belongs
        /// <example>api/TicketData/GetTicketSpot/1</example>
        /// <example>api/TicketData/GetTicketSpot/3</example>
        /// </summary>
        /// <param name="id">ID of the selected ticket</param>
        /// <returns>The parking spot to which current ticket belongs</returns>

        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetApplicationCourse(int id)
        {

            //Find the owner/user to which the current ticket belongs
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
        /// This method permits to update the selected ticket
        /// <example>api/TicketData/UpdateTicket/1</example>
        /// <example>api/TicketData/UpdateTicket/3</example>
        /// </summary>
        /// <param name="id">The ID of the ticket</param>
        /// <param name="Ticket">The current ticket itself</param>
        /// <returns>Saves the current ticket with new values to the database</returns>

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
        /// This method permits to add a new ticket to the database
        /// <example>POST: api/TicketData/AddTicket</example>
        /// </summary>
        /// <param name="ticket">The actual ticket to be added</param>
        /// <returns> It adds a new ticket to the database</returns>

        [HttpPost]
        [ResponseType(typeof(EmployeeApplicant))]
        public IHttpActionResult AddTicket(EmployeeApplicant EmployeeApplicant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeApplicant.Add(EmployeeApplicant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = EmployeeApplicant.EmployeeApplicantId }, EmployeeApplicant);
        }

        /// <summary>
        /// This method deletes the ticket which ID is given
        /// <example>api/TicketData/DeleteTicket/1</example>
        /// <example>api/TicketData/DeleteTicket/5</example>
        /// </summary>
        /// <param name="id">ID of the ticket to be removed</param>
        /// <returns></returns>
        /// 



        /// <summary>
        /// This method gets the list of all tikets from the database with their owner name and spotinformation
        /// <example> GET: api/TicketData/GetAllTickets </example>
        /// </summary>
        /// <returns> The list of tickets, the parking spot and user to which they belong to from the database</returns>

        [ResponseType(typeof(IEnumerable<ShowEmployeeApplicant>))]
        public IHttpActionResult GetAllApplications()
        {

            //List of the tickets from the database
            List<EmployeeApplicant> EmployeeApplicants = db.EmployeeApplicant.ToList();

            //Data transfer object to show information about the ticket
            List<ShowEmployeeApplicant> EmployeeApplicantDtos = new List<ShowEmployeeApplicant> { };

            foreach (var EmployeeApplicant in EmployeeApplicants)
            {
                ShowEmployeeApplicant EmployeeApplication = new ShowEmployeeApplicant();

                //Get the user to which the ticket belongs to
                User user = db.OurUsers.Where(c => c.EmployeeApplicants.Any(m => m.EmployeeApplicantId == EmployeeApplicant.EmployeeApplicantId)).FirstOrDefault();

                UserDto parentUser = new UserDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                //Get the parking spot of ticket
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
                    UserId = EmployeeApplicant.UserId,
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
        /// This method deletes the ticket object which ID is given
        /// <example>api/TicketData/DeleteTicket/1</example>
        /// <example>api/TicketData/DeleteTicket/3</example>
        /// </summary>
        /// <param name="id">ID of the ticket</param>
        /// <returns>Remove the ticket from the database</returns>

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