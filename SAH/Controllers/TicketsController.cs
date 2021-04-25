using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SAH.Models;
using SAH.Models.ModelViews;

namespace SAH.Controllers
{
    public class TicketsController : Controller
    {
        /*All the controllers can be automatically generated from:
         Controllers (folder)->Add->Controller-> MVC5 Controller with views, using Entity Framework->
         Add->Select the right Model and database, then give a name to the controller. Keep the 3 fields of Views ticked. 
         */
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static TicketsController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };

            client = new HttpClient(handler);

            //The user has to change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44378/api/");

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        }

        /// <summary>
        /// This method displays the list of all tickets with their users and parking spots
        /// <example>Tickets/TicketList</example>
        /// </summary>
        /// <returns>Tickets list with users and parking spots</returns>
        public ActionResult TicketList()
        {
            //Getting the list of all tickets with their information
            string url = "TicketData/GetAllTickets";

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ShowTicket> AllTickets = response.Content.ReadAsAsync<IEnumerable<ShowTicket>>().Result;
                return View(AllTickets);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method shows the information of the selected ticket
        /// <example>tickets/Details/1</example>
        /// <example>tickets/Details/4</example>
        /// </summary>
        /// <param name="id">ID of the selected ticket</param>
        /// <returns>Details of the ticket which ID is given</returns>

        public ActionResult Details(int id)
        {

            ShowTicket showTicket = new ShowTicket();

            //Get the current ticket from the database
            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                TicketDto SelectedTicket = response.Content.ReadAsAsync<TicketDto>().Result;
                showTicket.Ticket = SelectedTicket;

                //Get the user/owner of the selected ticket
                url = "TicketData/GetTicketUser/" + id;
                response = client.GetAsync(url).Result;
                ApplicationUserDto SelectedUser = response.Content.ReadAsAsync<ApplicationUserDto>().Result;
                showTicket.User = SelectedUser;

                //Get the parking spot of the selected ticket
                url = "TicketData/GetTicketSpot/" + id;
                response = client.GetAsync(url).Result;
                ParkingSpotDto SelectedSpot = response.Content.ReadAsAsync<ParkingSpotDto>().Result;
                showTicket.Spot = SelectedSpot;

                return View(showTicket);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method displays the field required to create a new ticket
        /// <example>// GET: Tickets/Create</example>
        /// </summary>
        /// <returns>Shows the fields required for the new ticket</returns>
        
        //[Authorize(Roles = "Visitor,Doctor,Admin")]
        public ActionResult Create()
        {
            //Get all the users for dropdown list
            EditTicket editTicket = new EditTicket();
            string url = "TicketData/GetUsers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ApplicationUserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<ApplicationUserDto>>().Result;
            editTicket.AllUsers = SelectedUsers;

            //Get all the parking spots for dropdown list
            url = "TicketData/GetParkingSpots";
            response = client.GetAsync(url).Result;

            IEnumerable<ParkingSpotDto> SelectedSpots = response.Content.ReadAsAsync<IEnumerable<ParkingSpotDto>>().Result;
            editTicket.AllSpots = SelectedSpots;

            return View(editTicket);
        }

        /// <summary>
        /// This method creates a new ticket object
        /// </summary>
        /// <param name="Ticket">Ticket to be created</param>
        /// <returns>Creates and saves the new ticket to the database</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
       // [Authorize(Roles = "Visitor,Doctor,Admin")]
        public ActionResult Create(Ticket Ticket)//
        {


            //Add a new ticket to the database
            string url = "TicketData/AddTicket";
            HttpContent content = new StringContent(jss.Serialize(Ticket));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //Redirect to the TicketList
                return RedirectToAction("TicketList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method allows to show the information of the selected ticket to be edited
        /// <example>Tickets/Edit/4</example>
        /// <example>Tickets/Edit/2</example>
        /// </summary>
        /// <param name="id">ID of the selected ticket</param>
        /// <returns>Shows the selected ticket in the view</returns>
        
        [Authorize(Roles = "Visitor,Doctor,Admin")]
        public ActionResult Edit(int id)
        {
            EditTicket newTicket = new EditTicket();

            //Get the selected ticket from the database
            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                TicketDto SelectedTicket = response.Content.ReadAsAsync<TicketDto>().Result;
                newTicket.Ticket = SelectedTicket;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get all users from the database for dropdown list
            url = "TicketData/GetUsers";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ApplicationUserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<ApplicationUserDto>>().Result;
                newTicket.AllUsers = SelectedUsers;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get all parking spots from the database for dropdown list
            url = "TicketData/GetParkingSpots";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ParkingSpotDto> SelectedSpots = response.Content.ReadAsAsync<IEnumerable<ParkingSpotDto>>().Result;
                newTicket.AllSpots = SelectedSpots;
            }
            else
            {
                return RedirectToAction("Error");
            }
            return View(newTicket);

        }

        /// <summary>
        /// This method edits the selected ticket 
        /// <example>POST: Tickets/Edit/1</example>
        /// </summary>
        /// <param name="id">ID of the selected ticket</param>
        /// <param name="Ticket">Selected ticket itself</param>
        /// <returns>Updates and saves the current ticket to the database</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Visitor,Doctor,Admin")]
        public ActionResult Edit(int id, Ticket Ticket)//
        {


            //Update and save the current ticket
            string url = "TicketData/UpdateTicket/" + id;

            HttpContent content = new StringContent(jss.Serialize(Ticket));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = Ticket.TicketId });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        /// <summary>
        /// This method shows the selected ticket
        /// <example>GET: Tickets/Delete/1</example>
        /// <example>GET: Tickets/Delete/3</example>
        /// </summary>
        /// <param name="id">ID of the selected ticket</param>
        /// <returns>Shows the current ticket</returns>
        // 
        [Authorize(Roles = "Visitor,Doctor,Admin")]
        public ActionResult Delete(int id)
        {
            //Get current ticket from the database
            string url = "TicketData/FindTicket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                Ticket SelectedTicket = response.Content.ReadAsAsync<Ticket>().Result;
                return View(SelectedTicket);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        /// <summary>
        /// This method removes the selected ticket from the database
        /// <example>POST: Tickets/Delete/2</example>
        ///  <example>POST: Tickets/Delete/5</example>
        /// </summary>
        /// <param name="id">Id of the selected ticket</param>
        /// <returns>Removes the selected ticket from the database</returns>
        // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Visitor,Doctor,Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Delete current ticket from database
            string url = "TicketData/DeleteTicket/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("TicketList");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
