using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ShowParkingSpot
    {
        //Information about the parking spot
        public ParkingSpotDto Spot { get; set; }

        //Information about all tickets for this spot
        public IEnumerable<TicketDto> AllTickets { get; set; }

    }
}