using SAH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SAH.Controllers
{
    public class AppointmentDataController : ApiController
    {
        //This variable is our database access point
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// A list of Appointment in the database
        /// </summary>
        /// <returns>The list of Appointments Posted</returns>
        /// <example>
        /// GET: AppointmentData/GetAppointments
        /// </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<AppointmentDto>))]
        public IHttpActionResult GetAppointments()
        {
            List<Appointment> appointments = db.Appointments.ToList();
            List<AppointmentDto> appointmentDtos = new List<AppointmentDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var appointment in appointments)
            {
                AppointmentDto newAppointment = new AppointmentDto
                {
                    AppointmentID = appointment.AppointmentID,
                    FirstName = appointment.FirstName,
                    MiddleName = appointment.MiddleName,
                    LastName = appointment.LastName,
                    Gender = appointment.Gender,
                    DateOfBirth = appointment.DateOfBirth,
                    Address = appointment.Address,
                    City = appointment.City,
                    Province = appointment.Province,
                    PostalCode = appointment.PostalCode,
                    Email = appointment.Email,
                    HelthCardNumber = appointment.HelthCardNumber,
                    PhoneNumber = appointment.PhoneNumber,
                    Note = appointment.Note,
                    PreferedTime = appointment.PreferedTime,
                    AppintmentDateTime = appointment.AppintmentDateTime,
                    IsFirstTimeVisit = appointment.IsFirstTimeVisit,
                    IsUrgent = appointment.IsUrgent,
                    Id = appointment.Id,
                    DepartmentID = appointment.DepartmentID
            };

                if (appointment.Id != null)
                {
                    //We get the doctor name from user table when role is doctor
                    newAppointment.DoctorName = appointment.FirstName + " " + appointment.LastName;
                }

                if (appointment.Department != null)
                {
                    newAppointment.DepartmentName = appointment.Department.DepartmentName;
                }

                appointmentDtos.Add(newAppointment);
            }

            return Ok(appointmentDtos);
        }

        /// <summary>
        /// Finds a particular Appointment in the database with a 200 status code. If the Sponsor is not found, return 404.
        /// </summary>
        /// <param name="id">The Appointment id</param>
        /// <returns>Information about the Appointment</returns>
        // <example>
        // GET: api/AppointmentData/FindAppointment/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<AppointmentDto>))]
        public IHttpActionResult FindAppointment(int ID)
        {
            Appointment appointment = db.Appointments.Find(ID);

            AppointmentDto appointmentDto = new AppointmentDto
            {
                AppointmentID = appointment.AppointmentID,
                FirstName = appointment.FirstName,
                MiddleName = appointment.MiddleName,
                LastName = appointment.LastName,
                Gender = appointment.Gender,
                DateOfBirth = appointment.DateOfBirth,
                Address = appointment.Address,
                City = appointment.City,
                Province = appointment.Province,
                PostalCode = appointment.PostalCode,
                Email = appointment.Email,
                HelthCardNumber = appointment.HelthCardNumber,
                PhoneNumber = appointment.PhoneNumber,
                Note = appointment.Note,
                PreferedTime = appointment.PreferedTime,
                AppintmentDateTime = appointment.AppintmentDateTime,
                IsFirstTimeVisit = appointment.IsFirstTimeVisit,
                IsUrgent = appointment.IsUrgent,
                DepartmentID = appointment.DepartmentID,
                Id = appointment.Id
            }; 
            
            if (appointment.Id != null)
            {
                //We get the doctor name from user table when role is doctor
                appointmentDto.DoctorName = appointment.FirstName + " " + appointment.LastName;
            }
            if (appointment.Department != null)
            {
                appointmentDto.DepartmentName = appointment.Department.DepartmentName;
            }

            return Ok(appointmentDto);
        }


        /// <summary>
        /// Update an appointment in the database, The past information is given
        /// </summary>
        /// <param name="Id">Appointment Id</param>
        /// <param name="Appointment">Appointment Object. Received a POST Data</param>
        /// <returns></returns>
        /// <example>
        /// PUT: api/appointmentdata/updateappointment/5
        /// </example>   
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult updateappointment(int id, [FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.AppointmentID)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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
        /// Add a new Appointment to the database
        /// </summary>
        /// <param name="Appointment">Sent a Post form Data</param>
        /// <returns>200 = successful. 404 = not successful</returns>
        /// <example> 
        /// POST: api/AppointmentData/AddAppointment
        /// </example>
        /// 
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult AddAppointment([FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return Ok(appointment.AppointmentID);
        }

        /// <summary>
        /// Delete a Appointment from the database
        /// </summary>
        /// <param name="Id">The Id from the Appointment to delete</param>
        /// <returns>200 = successful. 404 = not successful</returns>
        /// <example>
        /// POST: api/AppointmentData/DeleteAppointment/2
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
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
        /// Find a Appointment in the System
        /// </summary>
        /// <param name="Id">The Appointment Id</param>
        /// <returns>If the Appointment exists return true</returns>

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Any(e => e.AppointmentID == id);
        }
    }
}
