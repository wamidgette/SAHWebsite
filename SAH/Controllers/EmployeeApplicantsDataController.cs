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

namespace SAH.Controllers
{
    public class EmployeeApplicantsDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();

        // GET: api/EmployeeApplicantsData
        public IQueryable<EmployeeApplicant> GetEmployeeApplicant()
        {
            return db.EmployeeApplicant;
        }

        // GET: api/EmployeeApplicantsData/5
        [ResponseType(typeof(EmployeeApplicant))]
        public IHttpActionResult GetEmployeeApplicant(int id)
        {
            EmployeeApplicant employeeApplicant = db.EmployeeApplicant.Find(id);
            if (employeeApplicant == null)
            {
                return NotFound();
            }

            return Ok(employeeApplicant);
        }

        // PUT: api/EmployeeApplicantsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeApplicant(int id, EmployeeApplicant employeeApplicant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeApplicant.EmployeeApplicantId)
            {
                return BadRequest();
            }

            db.Entry(employeeApplicant).State = EntityState.Modified;

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

        // POST: api/EmployeeApplicantsData
        [ResponseType(typeof(EmployeeApplicant))]
        public IHttpActionResult PostEmployeeApplicant(EmployeeApplicant employeeApplicant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeApplicant.Add(employeeApplicant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employeeApplicant.EmployeeApplicantId }, employeeApplicant);
        }

        // DELETE: api/EmployeeApplicantsData/5
        [ResponseType(typeof(EmployeeApplicant))]
        public IHttpActionResult DeleteEmployeeApplicant(int id)
        {
            EmployeeApplicant employeeApplicant = db.EmployeeApplicant.Find(id);
            if (employeeApplicant == null)
            {
                return NotFound();
            }

            db.EmployeeApplicant.Remove(employeeApplicant);
            db.SaveChanges();

            return Ok(employeeApplicant);
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