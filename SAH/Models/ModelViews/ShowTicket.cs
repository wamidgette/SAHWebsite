using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ShowTicket
    {
        //Information about the ticket
        public TicketDto Ticket { get; set; }
        //User to which ticket belongs 
        public ApplicationUserDto User { get; set; }
        //Parking spot for which ticket is issued
        public ParkingSpotDto Spot { get; set; }
    }
}