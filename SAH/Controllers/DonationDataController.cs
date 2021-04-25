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
        /// GET: api/DonationData/GetDonationInfo
        /// </example>

        [ResponseType(typeof(IEnumerable<ListDonation>))]
        public IHttpActionResult GetDonationInfo()
        {
            //Join three tables
            //SELECT * FROM Donations, Users, Departments WHERE Donations.UserId = Users.UserId AND Donations.DepartmentId = Departments.DepartmentId 
            var AllInfos = db.Donations
                 .Include(d => d.ApplicationUser)
                 .Include(d => d.Department)
                 .ToList();

            //Initialize object to have all donation related data
            List<ListDonation> ModelView = new List<ListDonation> { };
            /*
            List<DonationDto> DonationDtos = new List<DonationDto>();
            List<ApplicationUserDto> UserDtos = new List<ApplicationUserDto>();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();
            ListDonation NewInfo = new ListDonation();
            */
            //Infomation to be exposed
            foreach (var info in AllInfos)
            {

                ListDonation NewInfo = new ListDonation
                {
                    DonationId = info.DonationId,
                    AmountOfDonation = info.AmountOfDonation,
                    PaymentMethod = info.PaymentMethod,
                    DonationDate = info.DonationDate,
                    //Id = info.Id,
                    DepartmentId = info.DepartmentId,
                    Id = info.ApplicationUser.Id,
                    FirstName = info.ApplicationUser.FirstName,
                    LastName = info.ApplicationUser.LastName,
                    DepartmentName = info.Department.DepartmentName
                    
                };
                
                /*
                DonationDto NewDonation = new DonationDto
                {
                    DonationId = info.DonationId,
                    AmountOfDonation = info.AmountOfDonation,
                    PaymentMethod = info.PaymentMethod,
                    DonationDate = info.DonationDate,
                    Id = info.Id,
                    DepartmentId = info.DepartmentId,
                };

                ApplicationUserDto NewUser = new ApplicationUserDto
                {
                    Id = info.ApplicationUser.Id,
                    FirstName = info.ApplicationUser.FirstName,
                    LastName = info.ApplicationUser.LastName
                };

                DepartmentDto NewDepartment = new DepartmentDto
                {
                    DepartmentId = info.Department.DepartmentId,
                    DepartmentName = info.Department.DepartmentName
                };

                DonationDtos.Add(NewDonation);
                UserDtos.Add(NewUser);
                DepartmentDtos.Add(NewDepartment);
                */
                ModelView.Add(NewInfo);
            }
            /*
            NewInfo.AllDonations = DonationDtos;
            NewInfo.AllUsers = UserDtos;
            NewInfo.AllDepartments = DepartmentDtos;
            */
            return Ok(ModelView);
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
                Id = Donation.Id,
                DepartmentId = Donation.DepartmentId
            };

            return Ok(DonationDto);
        }

        /// <summary>
        /// This will find a donation informaion by donor ID
        /// </summary>
        /// <param name="id">The Donor ID</param>
        /// <returns>
        /// A donation information includes donationId, amount of donation, payment method, donation date, department Id
        /// </returns>
        /// <example>
        /// GET: api/DonationData/FindDonationForDonor/5 
        /// </example>

        [HttpGet]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult FindDonationForDonor(string id)
        {
            //SELECET * FROM User, Donation WHERE U.UserID = D.UserID AND DonationId = id
            Donation Donation = db.Donations
                .Where(d => d.Id == id)
                .FirstOrDefault();

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
                Id = Donation.Id,
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
        [ResponseType(typeof(ApplicationUserDto))]
        public IHttpActionResult FindDonorForDonation(int id)
        {
            //SELECET * FROM User, Donation WHERE U.UserID = D.UserID AND DonationId = id
            ApplicationUser User = db.Users.Where(u => u.Donations.Any(d => d.DonationId == id)).FirstOrDefault();
            
            if (User == null)
            {
                return NotFound();
            }

            ApplicationUserDto UserDto = new ApplicationUserDto
            {
                Id = User.Id,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                Address = User.Address,
                PostalCode = User.PostalCode
            };
            return Ok(UserDto);
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin,Patient,Donor")]
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
        [Authorize(Roles = "Admin")]
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
