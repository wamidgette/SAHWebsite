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
using System.Diagnostics;

namespace SAH.Controllers
{
    public class SpecialityDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// Gets a list of specialities in the database
        /// </summary>
        /// <returns>A list of specialities with their ID</returns>
        /// <example>
        /// GET: api/SpecialityData/GetSpecialties
        /// </example>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<SpecialityDto>))]
        public IHttpActionResult GetSpecialities()
        {
            List<Speciality> Specialties = db.Specialities.ToList();
            List<SpecialityDto> SpecialityDtos = new List<SpecialityDto> { };

            Debug.WriteLine(Specialties);

            //Infomation to be exposed to the API
            foreach (var Speciality in Specialties)
            {
                SpecialityDto NewSpeciality = new SpecialityDto
                {
                    SpecialityId = Speciality.SpecialityId,
                    SpecialityName = Speciality.SpecialityName
                };
                SpecialityDtos.Add(NewSpeciality);
            }

            return Ok(SpecialityDtos);
        }

        /// <summary>
        /// Finds a particular speciality in the database by speciality ID.
        /// </summary>
        /// <param name="id">The speciality ID</param>
        /// <returns>Specialty ID and Speciality name. If the speciality is not found, return 404.</returns>
        /// <example>
        /// GET: api/SpecialtyData/FindSpeciality/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(SpecialityDto))]
        public IHttpActionResult FindSpeciality(int id)
        {
            //Find the data
            Speciality Speciality = db.Specialities.Find(id);

            //If not found, return 404
            if (Speciality == null)
            {
                return NotFound();
            }

            //Information to be return
            SpecialityDto SpecialityDto = new SpecialityDto
            {
                SpecialityId = Speciality.SpecialityId,
                SpecialityName = Speciality.SpecialityName
            };

            return Ok(SpecialityDto);
        }

        /// <summary>
        /// Updates a speciality in the database given information about the speciality.
        /// </summary>
        /// <param name="id">The specialtiy ID</param>
        /// <param name="speciality">A speciality object</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/SpecialityData/UpdateSpeciality/5
        /// FORM DATA: Speciality JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSpeciality(int id, [FromBody] Speciality speciality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != speciality.SpecialityId)
            {
                return BadRequest();
            }

            db.Entry(speciality).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialityExists(id))
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
        /// Adds a speciality to the database.
        /// </summary>
        /// <param name="speciality">A speciality object.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/SpecialityData/AddSpeciality
        /// FORM DATA: Speciality JSON Object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Speciality))]
        public IHttpActionResult AddSpeciality([FromBody] Speciality speciality)
        {
            //validation according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Specialities.Add(speciality);
            db.SaveChanges();

            return Ok(speciality.SpecialityId);
        }

        /// <summary>
        /// Deletes a speciality in the database
        /// </summary>
        /// <param name="id">The ID of the speciality to be deleted.</param>
        /// <returns>200 if successful. 404 if unsuccessful</returns>
        /// <example>
        /// POST: api/SpecialityData/DeleteSpeciality/5
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Speciality))]
        public IHttpActionResult DeleteSpeciality(int id)
        {
            Speciality speciality = db.Specialities.Find(id);
            if (speciality == null)
            {
                return NotFound();
            }

            db.Specialities.Remove(speciality);
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
        /// Finds a speciality in the system. Internal user only.
        /// </summary>
        /// <param name="id">The speciality ID</param>
        /// <returns>True if the speciality exists, false otherwise.</returns>
        private bool SpecialityExists(int id)
        {
            return db.Specialities.Count(e => e.SpecialityId == id) > 0;
        }
    }
}