using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAH.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public string NumberPlate { get; set; }
        public DateTime EntryTime { get; set; }
        public Decimal  Duration { get; set; }
        public Decimal Fees { get; set; }

        //A ticket belongs to a user e
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        //A ticket is linked to a parking spot
        [ForeignKey("ParkingSpot")]
        public int SpotId { get; set; }
        public virtual ParkingSpot ParkingSpot { get; set; }
    }

    public class TicketDto
    {
        public int TicketId { get; set; }
        public string NumberPlate { get; set; }
        public DateTime EntryTime { get; set; }
        public Decimal Duration { get; set; }
        public Decimal Fees { get; set; }

        //A ticket belongs to a user 
        public string Id { get; set; }

        //A ticket is linked to a parking spot
        public int SpotId { get; set; }
    }
}