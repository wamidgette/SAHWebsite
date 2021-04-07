using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAH.Models
{
    public class ParkingSpot
    {
        [Key]
        public int SpotId { get; set; }
        public string Zone { get; set; }
        public string SpotNumber { get; set; }
        public Boolean Status { get; set; }

        //A spot can have many tickets
        public ICollection<Ticket> Tickets { get; set; }
    }

    public class ParkingSpotDto
    {
        public int SpotId { get; set; }
        public string Zone { get; set; }
        public string SpotNumber { get; set; }
        public Boolean Status { get; set; }
    }
}