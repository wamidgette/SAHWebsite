using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class EditTicket
    {
        //Information about the ticket
        public TicketDto Ticket { get; set; }

        //Needed for a dropdownlist for users
        public IEnumerable<UserDto> AllUsers { get; set; }
        //Needed for a dropdownlist for parking spots
        public IEnumerable<ParkingSpotDto> AllSpots { get; set; }
    }
}