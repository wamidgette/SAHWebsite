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
    public class ApplicationController : Controller    {


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
            string url = "ApplicationData/GetAllApplications";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ShowApplication> SelectedApplications = response.Content.ReadAsAsync<IEnumerable<ShowApplication>>().Result;
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
            ChangeApplication ChangeApplication = new ChangeApplication();
            string url = "ApplicationData/GetUsers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<UserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            ChangeApplication.AllUsers = SelectedUsers;

            //Get all the jobs for dropdown list
            url = "ApplicationData/GetJobs";
            response = client.GetAsync(url).Result;

            IEnumerable<JobDto> SelectedJobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;
            ChangeApplication.Jobs = SelectedJobs;

            return View(ChangeApplication);
        }

        // POST: Application/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Application Application)
        {
            //Add a new application to the database
            string url = "ApplicationData/AddApplication";
            HttpContent content = new StringContent(jss.Serialize(Application));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //Redirect to the Application List
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
            ChangeApplication newApplication = new ChangeApplication();

            //Get the selected ticket from the database
            string url = "ApplicationData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                ApplicationDto SelectedApplication = response.Content.ReadAsAsync<ApplicationDto>().Result;
                newApplication.Application = SelectedApplication;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get all users from the database for dropdown list
            url = "ApplicationData/GetUsers";
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

            //Get all jobs from the database for dropdown list
            url = "ApplicationData/GetJobs";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<JobDto> SelectedJobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;
                newApplication.Jobs = SelectedJobs;
            }
            else
            {
                return RedirectToAction("Error");
            }
            return View(newApplication);

        }

        // POST: Application/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Application Application)
        {            
            string url = "ApplicationData/UpdateApplication/" + id;

            HttpContent content = new StringContent(jss.Serialize(Application));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = Application.ApplicationId });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Application/Delete/5
        [HttpGet]
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
                ApplicationDto SelectedApplication = response.Content.ReadAsAsync<ApplicationDto>().Result;
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

          /*  public ActionResult Error()
           {
                return View();
           } */
        }
    }
}
