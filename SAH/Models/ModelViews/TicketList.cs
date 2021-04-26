using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class TicketList
    {
       //Conditionally render all tickets for admin otherwise only current user ticket
        public bool isadmin { get; set; }
        //First name used to select user tickets
        public string firstname { get; set; }
        //Information about the ticket
        public IEnumerable<ShowTicket> AllTickets { get; set; }
    }
}