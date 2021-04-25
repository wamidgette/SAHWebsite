using System;
using System.IO;
using System.Web;
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
    [Authorize(Roles = "Admin")]
    public class DepartmentDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// Gets a list of departments in the database
        /// </summary>
        /// <returns>A list of departments with their ID</returns>
        /// <example>
        /// GET: api/DepartmentData/GetDepartments
        /// </example>
        [AllowAnonymous]
        [ResponseType(typeof(IEnumerable<DepartmentDto>))]
        public IHttpActionResult GetDepartments()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto> { };

            Debug.WriteLine(Departments);

            //Infomation to be exposed to the API
            foreach (var Department in Departments)
            {
                DepartmentDto NewDepartment = new DepartmentDto
                {
                    DepartmentId = Department.DepartmentId,
                    DepartmentName = Department.DepartmentName
                };
                DepartmentDtos.Add(NewDepartment);
            }

            return Ok(DepartmentDtos);
        }

        /// <summary>
        /// Finds a particular department in the database by department ID.
        /// </summary>
        /// <param name="id">The department ID</param>
        /// <returns>Department ID and Department name. If the department is not found, return 404.</returns>
        /// <example>
        /// GET: api/DepartmentData/FindDepartment/5
        /// </example>
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult FindDepartment(int id)
        {
            //Find the data
            Department Department = db.Departments.Find(id);

            //If not found, return 404
            if (Department == null)
            {
                return NotFound();
            }

            //Information to be return
            DepartmentDto DepartmentDto = new DepartmentDto
            {
                DepartmentId = Department.DepartmentId,
                DepartmentName = Department.DepartmentName
            };

            return Ok(DepartmentDto);
        }

        /// <summary>
        /// This will find a donor by department ID
        /// </summary>
        /// <param name="id">The department ID</param>
        /// <returns>
        /// Donor information iclude donor's first/last name
        /// </returns>
        /// <example>
        /// GET: api/DepartmentData/FindDonorsForDepartment/5 
        /// </example>
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ApplicationUserDto>))]
        public IHttpActionResult FindDonorsForDepartment(int id)
        {
            //SELECET * FROM Department, Donation, User WHERE Department.DepartmentId = Donation.DepartmentId AND DepartmentId = id
            var Donors = db.Donations
                .Include(d => d.Department)
                .Include(d => d.ApplicationUser)
                .Where(d => d.DepartmentId == id)
                .ToList();

            List<ApplicationUserDto> UserDtos = new List<ApplicationUserDto> { };

            if (Donors == null)
            {
                return NotFound();
            }

            foreach(var donor in Donors)
            {
                ApplicationUserDto NewDonor = new ApplicationUserDto
                {
                    Id = donor.ApplicationUser.Id,
                    FirstName = donor.ApplicationUser.FirstName,
                    LastName = donor.ApplicationUser.LastName
                };
                UserDtos.Add(NewDonor);

            }
            return Ok(UserDtos);
        }

        /// <summary>
        /// This will find a department informaion by donation ID
        /// </summary>
        /// <param name="id">The donation ID</param>
        /// <returns>
        /// A department id, name
        /// </returns>
        /// <example>
        /// GET: api/DepartmentData/FindDepartmentForDonation/5 
        /// </example>
        [AllowAnonymous]
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
        /// Updates a department in the database given information about the department.
        /// </summary>
        /// <param name="id">The department ID</param>
        /// <param name="department">A department object</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/DepartmentData/UpdateDepartment/5
        /// FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDepartment(int id, [FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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
        /// Adds a department to the database.
        /// </summary>
        /// <param name="department">A department object.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/DepartmentData/AddDepartment
        /// FORM DATA: Department JSON Object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Department))]
        public IHttpActionResult AddDepartment([FromBody] Department department)
        {
            //validation according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return Ok(department.DepartmentId);
        }

        /// <summary>
        /// Deletes a department in the database
        /// </summary>
        /// <param name="id">The ID of the department to be deleted.</param>
        /// <returns>200 if successful. 404 if unsuccessful</returns>
        /// <example>
        /// POST: api/DepartmentData/DeleteDepartment/5
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Department))]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
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
        /// Finds a department in the system. Internal user only.
        /// </summary>
        /// <param name="id">The department ID</param>
        /// <returns>True if the department exists, false otherwise.</returns>
        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }
    }
}