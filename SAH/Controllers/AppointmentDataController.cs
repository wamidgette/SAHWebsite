using SAH.Models;
using System;
using System.Collections.Generic;
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
                    DepartmentID = appointment.DepartmentID,
                    DepartmentName = appointment.Department.DepartmentName
                };

                if (appointment.User != null)
                {
                    newAppointment.UserId = appointment.User.UserId;
                    //We get the doctor name from user table when role is doctor
                    newAppointment.DoctorName = appointment.User.FirstName + " " + appointment.User.LastName;
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
        //[HttpGet]
        //[ResponseType(typeof(IEnumerable<AppointmentDto>))]
        //public IHttpActionResult FindAppointment(int ID)
        //{
        //    Appointment appointment = db.Appointments.Find(ID);

        //    AppointmentDto appointmentDto = new AppointmentDto
        //    {
        //        AppointmentID = appointment.AppointmentID,
        //        Question = faq.Question,
        //        Answer = faq.Answer,
        //        Publish = faq.Publish,
        //        DepartmentID = faq.DepartmentID
        //    };

        //    return Ok(faqDto);
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Find a Faq in the System
        /// </summary>
        /// <param name="Id">The Faq Id</param>
        /// <returns>If the team exists return true</returns>

        private bool FaqExists(int id)
        {
            return db.Faqs.Any(e => e.FaqID == id);
        }
    }
}
