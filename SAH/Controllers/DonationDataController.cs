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
    public class DonationDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// Gets a list of donations in the database
        /// </summary>
        /// <returns>A list of donations information includes ID, amount of donation, payment method, donation date, donor's first name and last name</returns>
        /// <example>
        /// GET: api/DonationData/GetDonations
        /// </example>
        [ResponseType(typeof(IEnumerable<DonationDto>))]
        public IHttpActionResult GetDonations()
        {
            //Trying to join three tables
            //SELECT * FROM Donations, Users, Departments WHERE Donations.UserId = Users.UserId AND Donations.DepartmentId = Departments.DepartmentId 
            /*
            List<Donation> Donations = db.Donations
                .Join(db.OurUsers,
                      donation => donation.UserId,
                      user => user.UserId,
                      (donation, user) => new
                      
                      {
                          UserId = donation.UserId,
                          DonationId = donation.DonationId,
                          DepartmentId = donation.DepartmentId,
                          AmountOfDonation = donation.AmountOfDonation,
                          DonationDate = donation.DonationDate,
                          FirstName = user.FirstName,
                          LastName = user.LastName
                      }
                )
                .Join(db.Departments,
                      donation => donation.DepartmentId,
                      department => department.DepartmentId,
                      (donation, department) => new
                      {
                          DepartmentId = donation.DepartmentId,
                          DepartmentName = department.DepartmentName
                      }
                 )
                .ToList();
              */  

            List<Donation> Donations = db.Donations.ToList();
            List<DonationDto> DonationDtos = new List<DonationDto> { };

            //Infomation to be exposed to the API
            foreach (var donation in Donations)
            {
                DonationDto NewDonation = new DonationDto
                {
                    DonationId = donation.DonationId,
                    AmountOfDonation = donation.AmountOfDonation,
                    PaymentMethod = donation.PaymentMethod,
                    DonationDate = donation.DonationDate,
                    UserId = donation.UserId,
                    DepartmentId = donation.DepartmentId
                };
                DonationDtos.Add(NewDonation);

            }

            return Ok(DonationDtos);
        }

        /// <summary>
        /// Finds a particular donation in the database by donation ID.
        /// </summary>
        /// <param name="id">The donation ID</param>
        /// <returns>Donation ID and Donation name. If the donation is not found, return 404.</returns>
        /// <example>
        /// GET: api/DonationData/FindDonation/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult FindDonation(int id)
        {
            //Find the data
            Donation Donation = db.Donations.Find(id);

            //If not found, return 404
            if (Donation == null)
            {
                return NotFound();
            }

            //Information to be return
            DonationDto DonationDto = new DonationDto
            {
                DonationId = Donation.DonationId,
                AmountOfDonation = Donation.AmountOfDonation,
                PaymentMethod = Donation.PaymentMethod,
                DonationDate = Donation.DonationDate,
                UserId = Donation.UserId,
                DepartmentId = Donation.DepartmentId
            };

            return Ok(DonationDto);
        }

        /// <summary>
        /// This will find a donor informaion by donation ID
        /// </summary>
        /// <param name="id">The donation ID</param>
        /// <returns>
        /// A user information including userId, firstname, lastname, email, phone, address, postalcode
        /// </returns>
        /// <example>
        /// GET: api/DonationData/FindDonorForDonation/5 
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult FindDonorForDonation(int id)
        {
            //SELECET * FROM User, Donation WHERE U.UserID = D.UserID AND DonationId = id
            User User = db.OurUsers.Where(u => u.Donations.Any(d => d.DonationId == id)).FirstOrDefault();
            if (User == null)
            {
                return NotFound();
            }

            UserDto UserDto = new UserDto
            {
                UserId = User.UserId,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                Phone = User.Phone,
                Address = User.Address,
                PostalCode = User.PostalCode
            };
            return Ok(UserDto);
        }

        /// <summary>
        /// This will find a department informaion by donation ID
        /// </summary>
        /// <param name="id">The donation ID</param>
        /// <returns>
        /// A department id, name
        /// </returns>
        /// <example>
        /// GET: api/DonationData/FindDepartmentForDonation/5 
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult FindDepartmentForDonation(int id)
        {
            //SELECET * FROM Department, Donation WHERE Department.DepartmentId = Donation.DepartmentId AND DonationId = id
            Department Department = db.Departments.Where(de => de.Donations.Any(don => don.DonationId == id)).FirstOrDefault();
            if (Department == null)
            {
                return NotFound();
            }

            DepartmentDto DepartmentDto = new DepartmentDto
            {
                DepartmentId = Department.DepartmentId,
                DepartmentName = Department.DepartmentName
            };
            return Ok(DepartmentDto);
        }

        /// <summary>
        /// Updates a donation in the database given information about the donation.
        /// </summary>
        /// <param name="id">The donation ID</param>
        /// <param name="donation">A donation object</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/DonationData/UpdateDonation/5
        /// FORM DATA: Donation JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDonation(int id, [FromBody] Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donation.DonationId)
            {
                return BadRequest();
            }

            db.Entry(donation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
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
        /// Adds a donation to the database.
        /// </summary>
        /// <param name="donation">A donation object.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/DonationData/AddDonation
        /// FORM DATA: Donation JSON Object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Donation))]
        public IHttpActionResult AddDonation([FromBody] Donation donation)
        {
            //validation according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donations.Add(donation);
            db.SaveChanges();

            return Ok(donation.DonationId);
        }

        /// <summary>
        /// Deletes a donation in the database
        /// </summary>
        /// <param name="id">The ID of the donation to be deleted.</param>
        /// <returns>200 if successful. 404 if unsuccessful</returns>
        /// <example>
        /// POST: api/DonationData/DeleteDonation/5
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Donation))]
        public IHttpActionResult DeleteDonation(int id)
        {
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }

            db.Donations.Remove(donation);
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
        /// Finds a donation in the system. Internal user only.
        /// </summary>
        /// <param name="id">The donation ID</param>
        /// <returns>True if the donation exists, false otherwise.</returns>
        private bool DonationExists(int id)
        {
            return db.Donations.Count(e => e.DonationId == id) > 0;
        }
    }
}