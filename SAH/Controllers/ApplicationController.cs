using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using SAH.Models;
using SAH.Models.ModelViews;
using System.IO;

namespace SAH.Controllers
{
    public class ApplicationController : Controller
    {


        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static ApplicationController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44378/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }
        // GET: Application/List
        public ActionResult List()
        {
            string url = "ApplicationData/GetApplications";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ApplicationDto> SelectedApplications = response.Content.ReadAsAsync<IEnumerable<ApplicationDto>>().Result;
                return View(SelectedApplications);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Application/Details/5
        public ActionResult Details(int id)
        {
            ShowApplication showApplication = new ShowApplication();

            //Find the application from the database
            string url = "ApplicationData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                ApplicationDto SelectedApplication = response.Content.ReadAsAsync<ApplicationDto>().Result;
                showApplication.Application = SelectedApplication;

                //Associated Application with User
                url = "ApplicationData/GetApplicationUser/" + id;
                response = client.GetAsync(url).Result;
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                showApplication.User = SelectedUser;

                //Associated application with Job
                url = "ApplicationData/GetApplicationJob/" + id;
                response = client.GetAsync(url).Result;
                JobDto SelectedJob = response.Content.ReadAsAsync<JobDto>().Result;
                showApplication.Job = SelectedJob;

                return View(showApplication);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Application/Create
        public ActionResult Create()
        {
            //Get all the users for dropdown list
            changeApplication changeApplication = new changeApplication();
            string url = "ApplicationData/GetUsers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<UserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            changeApplication.AllUsers = SelectedUsers;

            //Get all the parking spots for dropdown list
            url = "ApplicationData/GetJobs";
            response = client.GetAsync(url).Result;

            IEnumerable<JobDto> SelectedJobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;
            changeApplication.Jobs = SelectedJobs;

            return View(changeApplication);
        }

        // POST: Application/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Application Application)
        {
            //Add a new ticket to the database
            string url = "ApplicationData/AddApplication";
            HttpContent content = new StringContent(jss.Serialize(Application));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //Redirect to the TicketList
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Application/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Application/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Application/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            //Get current ticket from the database
            string url = "ApplicationData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                Application SelectedApplication = response.Content.ReadAsAsync<Application>().Result;
                return View(SelectedApplication);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Application/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "Applicationdata/DeleteApplication/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
