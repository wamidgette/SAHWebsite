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
    public class ParkingSpotsController : Controller
    {
        /*All the controllers can be automatically generated from:
        Controllers (folder)->Add->Controller-> MVC5 Controller with views, using Entity Framework->
        Add->Select the right Model and database, then give a name to the controller. Keep the 3 fields of Views ticked. 
        */
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static ParkingSpotsController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //User must change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44378/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        }

        /// <summary>
        /// This method returns the ParkingSpot list to view
        /// <example>ParkingSpots/ParkingSpotList</example>
        /// </summary>
        /// <returns>ParkingSpot list</returns>

        public ActionResult ParkingSpotList()
        {
            string url = "ParkingSpotData/GetParkingSpots";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ParkingSpot> SelectedSpots = response.Content.ReadAsAsync<IEnumerable<ParkingSpot>>().Result;
                return View(SelectedSpots);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method gives details about the selected Parking Spot. It shows all tickets linked to that Parking Spot.
        /// <example>ParkingSpots/Details/5</example>
        /// <example>ParkingSpots/Details/2</example>
        /// </summary>
        /// <param name="id">The ID of the selected ParkingSpot object</param>
        /// <returns>ParkingSpot object details with pupils and modules</returns>
        /// 
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            //Model used to combine a Parking Spot object and its tickets
            ShowParkingSpot ViewModel = new ShowParkingSpot();

            //Get the current ParkingSpot object
            string url = "ParkingSpotData/FindParkingSpot/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                ParkingSpotDto SelectedSpots = response.Content.ReadAsAsync<ParkingSpotDto>().Result;
                ViewModel.Spot = SelectedSpots;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get the tickets which are linked to the current Parking Spot object
            url = "ParkingSpotData/GetSpotTickets/" + id;
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Put data into ticket data transfer object
                IEnumerable<TicketDto> SelectedTickets = response.Content.ReadAsAsync<IEnumerable<TicketDto>>().Result;
                ViewModel.AllTickets = SelectedTickets;
            }
            else
            {
                return RedirectToAction("Error");
            }

            return View(ViewModel);
        }

        /// <summary>
        /// This method shows the fields of the Parking Spot object to be created
        /// </summary>
        /// <returns>The current Parking Spot fields to the view</returns>
        /// 
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// This method saves the new parking spot to the database
        /// <example>POST: ParkingSpots/Create</example>
        /// </summary>
        /// <param name="newSpot">The current ParkingSpot object to be saved</param>
        /// <returns></returns>
        // 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ParkingSpot newSpot)
        {
            //Saving the new Parking Spot object to the database
            string url = "ParkingSpotData/AddParkingSpot";
            HttpContent content = new StringContent(jss.Serialize(newSpot));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                //Go back to ParkingSpots list
                return RedirectToAction("ParkingSpotList");

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method shows the fields of the parking spot element to edit which ID is provided
        /// <example>GET: ParkingSpots/Edit/5 </example>
        /// </summary>
        /// <param name="id">Id of the selected Parking Spot object</param>
        /// <returns>Displays the selected Parking Spot object</returns>
        // 
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "ParkingSpotData/FindParkingSpot/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                ParkingSpot SelectedSpot = response.Content.ReadAsAsync<ParkingSpot>().Result;
                return View(SelectedSpot);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// This method is used to edit and save the selected ParkingSpot object
        /// <example>POST: ParkingSpots/Edit/5</example>
        /// /// <example>POST: ParkingSpots/Edit/1</example>
        /// </summary>
        /// <param name="id">ID of the Parking Spot object to be edited</param>
        /// <param name="currentSpot"></param>
        /// <returns>Modifies and Saves the selected Parking Spot to the database</returns>
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, ParkingSpot currentSpot)
        {
            //Update and save the ParkingSpot which ID is given
            string url = "ParkingSpotData/UpdateParkingSpot/" + id;

            HttpContent content = new StringContent(jss.Serialize(currentSpot));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        /// <summary>
        /// This method shows the information about the Parking Spot  before deletion
        /// <example>GET: ParkingSpots/Delete/5</example>
        /// <example>GET: ParkingSpots/Delete/1</example>
        /// </summary>
        /// <param name="id">ID of the selected Parking Spot</param>
        /// <returns>Show the selected Parking Spot </returns>
        // 
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            //Getting the ParkingSpot which id is given
            string url = "ParkingSpotData/FindParkingSpot/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                ParkingSpot SelectedSpot = response.Content.ReadAsAsync<ParkingSpot>().Result;
                return View(SelectedSpot);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        /// <summary>
        /// This method removes the selected parking spot from the database
        /// <example>POST: ParkingSpots/Delete/5</example>
        /// <example>POST: ParkingSpots/Delete/2</example>
        /// </summary>
        /// <param name="id">ID of the selected Parking Spot </param>
        /// <returns>Remove the Parking Spot from the database</returns>
                // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "ParkingSpotData/DeleteParkingSpot/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("ParkingSpotList");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

    }
}
