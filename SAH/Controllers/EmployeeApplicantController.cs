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
    public class EmployeeApplicantController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static EmployeeApplicantController()
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
        /// This method displays the list of all employee applications for courses
        /// <example>Tickets/TicketList</example>
        /// </summary>
        /// <returns>Tickets list with users and parking spots</returns>
        public ActionResult List()
        {
            //Getting the list of all tickets with their information
            string url = "EmployeeApplicantData/GetAllApplications";

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ShowEmployeeApplicant> AllApplications = response.Content.ReadAsAsync<IEnumerable<ShowEmployeeApplicant>>().Result;
                return View(AllApplications);
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

            ShowEmployeeApplicant ShowEmployeeApplicant = new ShowEmployeeApplicant();

            //Get the current ticket from the database
            string url = "EmployeeApplicantData/FindApplications/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                EmployeeApplicantDto SelectedApplication = response.Content.ReadAsAsync<EmployeeApplicantDto>().Result;
                ShowEmployeeApplicant.EmployeeApplicant = SelectedApplication;

                //Get the user/owner of the selected ticket
                url = "EmployeeApplicantData/GetCoursesbyApplicants/" + id;
                response = client.GetAsync(url).Result;
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ShowEmployeeApplicant.User = SelectedUser;

                //Get the parking spot of the selected ticket
                url = "EmployeeApplicantData/GetApplicationCourse/" + id;
                response = client.GetAsync(url).Result;
                CoursesDto SelectedCourse = response.Content.ReadAsAsync<CoursesDto>().Result;
                ShowEmployeeApplicant.Courses = SelectedCourse;

                return View(ShowEmployeeApplicant);
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

        public ActionResult Create()
        {
            //Get all the users for dropdown list
            EditEmployeeApplicant editApplication = new EditEmployeeApplicant();
            string url = "EmployeeApplicantData/GetUsers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<UserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            editApplication.AllUsers = SelectedUsers;

            //Get all the parking spots for dropdown list
            url = "EmployeeApplicantData/GetCourses";
            response = client.GetAsync(url).Result;

            IEnumerable<CoursesDto> SelectedCourses = response.Content.ReadAsAsync<IEnumerable<CoursesDto>>().Result;
            editApplication.AllCourses = SelectedCourses;

            return View(editApplication);
        }
        /// <summary>
        /// This method creates a new ticket object
        /// </summary>
        /// <param name="Ticket">Ticket to be created</param>
        /// <returns>Creates and saves the new ticket to the database</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeApplicant EmployeeApplicant)//
        {


            //Add a new ticket to the database
            string url = "EmployeeApplicantData/AddApplication";
            HttpContent content = new StringContent(jss.Serialize(EmployeeApplicant));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //Redirect to the TicketList
                return RedirectToAction("EmployeeApplicantList");
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

        public ActionResult Edit(int id)
        {
            EditEmployeeApplicant newApplication = new EditEmployeeApplicant();

            //Get the selected ticket from the database
            string url = "EmployeeApplicantData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                EmployeeApplicantDto SelectedEmployeeApplicant = response.Content.ReadAsAsync<EmployeeApplicantDto>().Result;
                return View(SelectedEmployeeApplicant);
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get all users from the database for dropdown list
            url = "EmployeeApplicantData/GetUsers";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<UserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
                newApplication.AllUsers = SelectedUsers;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get all parking spots from the database for dropdown list
            url = "EmployeeApplicantData/GetCourses";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<CoursesDto> SelectedCourses = response.Content.ReadAsAsync<IEnumerable<CoursesDto>>().Result;
                newApplication.AllCourses = SelectedCourses;
            }
            else
            {
                return RedirectToAction("Error");
            }
            return View(newApplication);

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
        public ActionResult Edit(int id, EmployeeApplicant EmployeeApplicant)//
        {


            //Update and save the current ticket
            string url = "EmployeeApplicantData/UpdateApplication/" + id;

            HttpContent content = new StringContent(jss.Serialize(EmployeeApplicant));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = EmployeeApplicant.EmployeeApplicantId });
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

        public ActionResult Delete(int id)
        {
            //Get current ticket from the database
            string url = "EmployeeApplicantData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                EmployeeApplicant SelectedApplication = response.Content.ReadAsAsync<EmployeeApplicant>().Result;
                return View(SelectedApplication);
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
        public ActionResult DeleteConfirmed(int id)
        {
            //Delete current ticket from database
            string url = "EmployeeApplicantData/DeleteApplication/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("EmployeeApplicantList");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
    }
}