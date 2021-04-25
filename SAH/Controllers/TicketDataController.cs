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
    public class TicketDataController : ApiController
    {
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// This method gets from the database the list of the all tickets
        /// <example> GET: api/TicketData/GetTickets </example>
        /// </summary>
        /// <returns> The list of tickets from the database</returns>

        //[ResponseType(typeof(IEnumerable<ParkingSpotDto>))]
        //public IHttpActionResult GetTickets()
        //{
        //    //Getting the list of tickets  objects from the databse
        //    List<Ticket> Tickets = db.Tickets.ToList();

        //    //Here a data transfer model is used to keep only the information to be displayed about a parking spot object
        //    List<TicketDto> TicketDtos = new List<TicketDto> { };

        //    //Transfering Ticket to data transfer object
        //    foreach (var Ticket in Tickets)
        //    {
        //        TicketDto NewTicket = new TicketDto
        //        {
        //            TicketId = Ticket.TicketId,
        //            NumberPlate = Ticket.NumberPlate,
        //            EntryTime = Ticket.EntryTime,
        //            Duration = Ticket.Duration,
        //            Fees = 5*Ticket.Duration,       // Ticket.Fees=5*Ticket.Duration
        //            Id = Ticket.Id,
        //            SpotId = Ticket.SpotId
        //        };
        //        TicketDtos.Add(NewTicket);
        //    }

        //    return Ok(TicketDtos);
        //}

        /// <summary>
        /// This method allows getting the specified ticket
        /// <example>GET: api/TicketData/FindTicket/5</example>
        /// <example>GET: api/TicketData/FindTickett/2</example>
        /// </summary>
        /// <param name="id"> ID of the selected ticket</param>
        /// <returns> This method returns the ticket which id is given</returns>

        [HttpGet]
        [ResponseType(typeof(TicketDto))]
        public IHttpActionResult FindTicket(int id)
        {
            Ticket Ticket = db.Tickets.Find(id);

            if (Ticket == null)
            {
                return NotFound();
            }

            //A data transfer object model used to show only most important information about the ticket
            TicketDto TempTicket = new TicketDto
            {
                TicketId = Ticket.TicketId,
                NumberPlate = Ticket.NumberPlate,
                EntryTime = Ticket.EntryTime,
                Duration = Ticket.Duration,
                Fees = 5 * Ticket.Duration,
                Id = Ticket.Id,
                SpotId = Ticket.SpotId
            };

            return Ok(TempTicket);

        }


        /// <summary>
        /// This method gets all the users from the table
        /// <example>GET: api/TicketData/GetUsers</example>
        /// <example>GET: api/TicketData/GetUsers</example>
        /// </summary>
        /// <returns>The list of all users</returns>

        [ResponseType(typeof(IEnumerable<ApplicationUserDto>))]
        public IHttpActionResult GetUsers()
        {
            //List of all users who potentially use the parking
            List<ApplicationUser> Users = db.Users.ToList();
            List<ApplicationUserDto> ApplicationUserDtos = new List<ApplicationUserDto> { };

            foreach (var User in Users)
            {
                ApplicationUserDto NewUser = new ApplicationUserDto
                {
                    Id = User.Id,
                    FirstName = User.FirstName,
                    LastName = User.LastName
                };
                ApplicationUserDtos.Add(NewUser);
            }

            return Ok(ApplicationUserDtos);
        }


        /// <summary>
        /// This method gets from the database the list of the all parking spots
        /// <example> GET: api/TicketData/GetParkingSpots </example>
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
        /// This method provides the user to which the current ticket belongs
        /// <example>api/TicketData/GetTicketUser/1</example>
        /// <example>api/TicketData/GetTicketUser/3</example>
        /// </summary>
        /// <param name="id">ID of the selected ticket</param>
        /// <returns>The user to which current ticket belongs</returns>

        [ResponseType(typeof(ApplicationUserDto))]
        public IHttpActionResult GetTicketUser(int id)
        {

            //Find the owner/user to which the current ticket belongs
            ApplicationUser user = db.Users.Where(u => u.Tickets.Any(t => t.TicketId == id)).FirstOrDefault();

            //In case this user does not exist
            if (user == null)
            {

                return NotFound();
            }

            ApplicationUserDto OwnerUser = new ApplicationUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
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

        [ResponseType(typeof(ApplicationUserDto))]
        public IHttpActionResult GetTicketSpot(int id)
        {

            //Find the owner/user to which the current ticket belongs
            ParkingSpot Spot = db.Spots.Where(s => s.Tickets.Any(t => t.TicketId == id)).FirstOrDefault();

            //In case this user does not exist
            if (Spot == null)
            {

                return NotFound();
            }

            ParkingSpotDto ParentSpot = new ParkingSpotDto
            {
                SpotId = Spot.SpotId,
                Zone = Spot.Zone,
                SpotNumber = Spot.SpotNumber,
                Status = Spot.Status
            };

            return Ok(ParentSpot);
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
        public IHttpActionResult UpdateTicket(int id, Ticket Ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Ticket.TicketId)
            {
                return BadRequest();
            }

            db.Entry(Ticket).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult AddTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tickets.Add(ticket);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ticket.TicketId }, ticket);
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

        [ResponseType(typeof(IEnumerable<ShowTicket>))]
        public IHttpActionResult GetAllTickets()
        {

            //List of the tickets from the database
            List<Ticket> Tickets = db.Tickets.ToList();

            //Data transfer object to show information about the ticket
            List<ShowTicket> TicketDtos = new List<ShowTicket> { };

            foreach (var Ticket in Tickets)
            {
                ShowTicket ticket = new ShowTicket();

                //Get the user to which the ticket belongs to
                ApplicationUser user = db.Users.Where(u => u.Tickets.Any(t => t.TicketId == Ticket.TicketId)).FirstOrDefault();

                ApplicationUserDto parentUser = new ApplicationUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                //Get the parking spot of ticket
                ParkingSpot Spot = db.Spots.Where(s => s.Tickets.Any(t => t.TicketId == Ticket.TicketId)).FirstOrDefault();

                ParkingSpotDto spot = new ParkingSpotDto
                {
                    SpotId = Spot.SpotId,
                    Zone = Spot.Zone,
                    SpotNumber = Spot.SpotNumber,
                    Status = Spot.Status
                };



                TicketDto NewTicket = new TicketDto
                {
                    TicketId = Ticket.TicketId,
                    NumberPlate = Ticket.NumberPlate,
                    EntryTime = Ticket.EntryTime,
                    Duration = Ticket.Duration,
                    Fees = 5 * Ticket.Duration,
                    Id = Ticket.Id,
                    SpotId = Ticket.SpotId
                };

                ticket.Ticket = NewTicket;
                ticket.Spot = spot;
                ticket.User = parentUser;
                TicketDtos.Add(ticket);
            }

            return Ok(TicketDtos);
        }


        /// <summary>
        /// This method deletes the ticket object which ID is given
        /// <example>api/TicketData/DeleteTicket/1</example>
        /// <example>api/TicketData/DeleteTicket/3</example>
        /// </summary>
        /// <param name="id">ID of the ticket</param>
        /// <returns>Remove the ticket from the database</returns>

        [HttpPost]
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult DeleteTicket(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            db.Tickets.Remove(ticket);
            db.SaveChanges();

            return Ok(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return db.Tickets.Count(e => e.TicketId == id) > 0;
        }
    }
}