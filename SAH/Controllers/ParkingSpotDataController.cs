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
    //This controller is generated from API Controllers using EF linked to the ParkingSpots table
    public class ParkingSpotDataController : ApiController
    {

        //This is the variable used to access the database
        private SAHDataContext db = new SAHDataContext();


        /// <summary>
        /// This method gets from the database the list of the all parking spots
        /// <example> GET: api/ParkingSpotData/GetParkingSpots </example>
        /// </summary>
        /// <returns> The list of parking spots from the database</returns>
      
        [ResponseType(typeof(IEnumerable<ParkingSpotDto>))]
        public IHttpActionResult GetParkingSpots()
        {
            //Getting the list of parking spots  objects from the databse
            List<ParkingSpot> ParkingSpots = db.Spots.ToList();

            //Here a data transfer model is used to keep only the information to be displayed about a parking spot object
            List<ParkingSpotDto> ParkingSpotDtos = new List<ParkingSpotDto> { };

            //Transfering parking spot to data transfer object
            foreach (var Spot in ParkingSpots)
            {
                ParkingSpotDto NewSpot = new ParkingSpotDto
                {
                    SpotId = Spot.SpotId,
                    Zone = Spot.Zone,
                    SpotNumber = Spot.SpotNumber,
                    Status = Spot.Status
                };
                ParkingSpotDtos.Add(NewSpot);
            }

            return Ok(ParkingSpotDtos);
        }

        /// <summary>
        /// This method allows getting the specified parking spot
        /// <example>GET: api/ParkingSpotData/FindParkingSpot/5</example>
        /// <example>GET: api/ParkingSpotData/FindParkingSpot/2</example>
        /// </summary>
        /// <param name="id"> ID of the selected parking spot</param>
        /// <returns> This method returns the parking spot  which id is given</returns>
       
        [HttpGet]
        [ResponseType(typeof(ParkingSpotDto))]
        public IHttpActionResult FindParkingSpot(int id)
        {
            ParkingSpot ParkingSpot = db.Spots.Find(id);

            if (ParkingSpot == null)
            {
                return NotFound();
            }

            //A data transfer object model used to show only most important information about the parking spot
            ParkingSpotDto TempSpot = new ParkingSpotDto
            {
                SpotId = ParkingSpot.SpotId,
                Zone = ParkingSpot.Zone,
                SpotNumber = ParkingSpot.SpotNumber,
                Status = ParkingSpot.Status
            };

            return Ok(TempSpot);

        }


        /// <summary>
        /// This method gets all the tickets issued for the current parking spot
        /// <example>GET: api/ParkingSpotData/GetSpotTickets/1</example>
        /// <example>GET: api/ParkingSpotData/GetSpotTickets/3</example>
        /// </summary>
        /// <param name="id">ID of the selected parking spot object</param>
        /// <returns>The list of tickets issued for the parking spot object which ID is given</returns>

        [ResponseType(typeof(IEnumerable<TicketDto>))]
        public IHttpActionResult GetSpotTickets(int id)
        {
            //List of all pupils registered in the current Classe (course)
            List<Ticket> Tickets = db.Tickets.Where(p => p.SpotId == id).ToList();
            List<TicketDto> TicketDtos = new List<TicketDto> { };

            foreach (var Ticket in Tickets)
            {
                TicketDto NewTicket = new TicketDto
                {
                    TicketId = Ticket.TicketId,
                    NumberPlate = Ticket.NumberPlate,
                    EntryTime = Ticket.EntryTime,
                    Duration = Ticket.Duration,
                    Fees = Ticket.Fees,
                    UserId = Ticket.UserId,
                    SpotId=Ticket.SpotId
                };
                TicketDtos.Add(NewTicket);
            }

            return Ok(TicketDtos);
        }



        /// <summary>
        /// This method permits to update the selected parking spot
        /// <example>api/ParkingSpotData/UpdateParkingSpot/1</example>
        /// <example>api/ParkingSpotData/UpdateParkingSpot/3</example>
        /// </summary>
        /// <param name="id">The ID of the parking spot</param>
        /// <param name="Spot">The current parking spot itself</param>
        /// <returns>Saves the current parking spot with new values to the database</returns>
      
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateParkingSpot(int id, ParkingSpot Spot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Spot.SpotId)
            {
                return BadRequest();
            }

            db.Entry(Spot).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkingSpotExists(id))
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
        /// This method permits to add a new parking spot to the database
        /// <example>POST: api/ParkingSpotData/AddParkingSpot</example>
        /// </summary>
        /// <param name="parkingSpot">The actual parking spot to be added</param>
        /// <returns> It adds a new parking spot to the database</returns>

        [HttpPost]
        [ResponseType(typeof(ParkingSpot))]
        public IHttpActionResult AddParkingSpot(ParkingSpot parkingSpot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Spots.Add(parkingSpot);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = parkingSpot.SpotId }, parkingSpot);
        }


        /// <summary>
        /// This method deletes the parking spot which ID is given
        /// <example>api/ParkingSpotData/DeleteParkingSpot/1</example>
        /// <example>api/ParkingSpotData/DeleteParkingSpot/5</example>
        /// </summary>
        /// <param name="id">ID of the parking spot</param>
        /// <returns>Deletes the parking spot from the database</returns>
       
        [HttpPost]
        [ResponseType(typeof(ParkingSpot))]
        public IHttpActionResult DeleteParkingSpot(int id)
        {
            ParkingSpot parkingSpot = db.Spots.Find(id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            db.Spots.Remove(parkingSpot);
            db.SaveChanges();

            return Ok(parkingSpot);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParkingSpotExists(int id)
        {
            return db.Spots.Count(e => e.SpotId == id) > 0;
        }
    }
}