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
        //Http Client is the proper way to connect to a webapi
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static ApplicationController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //changed to match local port
            client.BaseAddress = new Uri("https://localhost:44378/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }

        // GET: Application/List
        /// Reference: Varsity Project by Christine Bittle - Player Data Controllers
        public ActionResult List()
        {
            string url = "ApplicationData/GetAllApplications";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                //Using ShowApplication View Model
                IEnumerable<ShowApplication> SelectedApplications = response.Content.ReadAsAsync<IEnumerable<ShowApplication>>().Result;
                return View(SelectedApplications);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Application/Details/5
        /// Reference: Varsity Project by Christine Bittle - Player Data Controllers
        public ActionResult Details(int id)
        {
            ShowApplication ModelView = new ShowApplication();
            //Using the Show Application View Model
            //Find the application by Id from the database
            string url = "ApplicationData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Response 200 code if it is ok
            if (response.IsSuccessStatusCode)
            {
                ApplicationDto SelectedApplication = response.Content.ReadAsAsync<ApplicationDto>().Result;
                ModelView.Application = SelectedApplication;

                //Errors n/a if it is null
                //Associated Application with User
                url = "ApplicationData/GetUserForApplication/" + id;
                response = client.GetAsync(url).Result;
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ModelView.User = SelectedUser;

                //Errors n/a if it is null
                //Associated application with Job
                url = "ApplicationData/GetJobForApplication/" + id;
                response = client.GetAsync(url).Result;
                JobDto SelectedJob = response.Content.ReadAsAsync<JobDto>().Result;
                ModelView.Job = SelectedJob;

                return View(ModelView);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Application/Create
        /// Reference: Varsity Project by Christine Bittle - Player Data Controllers
        public ActionResult Create()
        {
            //Using an Update Application View Model
            UpdateApplication ModelView = new UpdateApplication();

            //Get Jobs from the Job Data controllers in order to create a dropdown menu
            //Jobs options to link the application
            string url = "JobData/GetJobs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<JobDto> SelectedJobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;
            ModelView.Jobs = SelectedJobs;

            //Get all the Users from the UserData controllers to create a dropdown menu
            //Users to link the application
            url = "UserData/GetUsers";
            response = client.GetAsync(url).Result;

            IEnumerable<UserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            ModelView.Users = SelectedUsers;

            return View(ModelView);
        }

        // POST: Application/Create
        /// Reference: Varsity Project by Christine Bittle - Player Data Controllers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Application ApplicationInfo)
        {
            //Add a new application to the database (POST)
            string url = "ApplicationData/AddApplication";
            Debug.WriteLine(jss.Serialize(ApplicationInfo));
            HttpContent content = new StringContent(jss.Serialize(ApplicationInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //Redirect to the Application List Page
                //int ApplicationId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List");
            }
            else
            {   //Redirect to the Error Page if it an error occur or it is unsuccessful
                return RedirectToAction("Error");
            }
        }

        // GET: Application/Edit/5
        /// Reference: Varsity Project by Christine Bittle - Player Data Controllers
        public ActionResult Edit(int id)
        {
            UpdateApplication ModelView = new UpdateApplication();

            //Get the selected application from the database
            string url = "ApplicationData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                ApplicationDto SelectedApplication = response.Content.ReadAsAsync<ApplicationDto>().Result;
                ModelView.Application = SelectedApplication;

                string urlUs = "UserData/GetUsers";
                response = client.GetAsync(urlUs).Result;
                IEnumerable<UserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
                ModelView.Users = SelectedUsers;

                string urlJb = "JobData/GetJobs";
                response = client.GetAsync(urlJb).Result;
                IEnumerable<JobDto> SelectedJobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;
                ModelView.Jobs = SelectedJobs;

                return View(ModelView);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // POST: Application/Edit/5
        /// Reference: Varsity Project by Christine Bittle - Player Data Controllers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Application ApplicationInfo)
        {
            string url = "ApplicationData/UpdateApplication/" + id;

            Debug.WriteLine(jss.Serialize(ApplicationInfo));
            HttpContent content = new StringContent(jss.Serialize(ApplicationInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                //Redirect to the list if it is successful
                return RedirectToAction("Details", new { id = id });
            }
            else
            {  //Unsuscessful = error page
                return RedirectToAction("Error");
            }

        }

        // GET: Application/DeleteConfirm/5
        /// Reference: Varsity Project by Christine Bittle - Player Data Controllers
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            //Get-Find requested application from the database
            string url = "ApplicationData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Put data into application data transfer object
                ApplicationDto SelectedApplication = response.Content.ReadAsAsync<ApplicationDto>().Result;
                return View(SelectedApplication);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Application/Delete/5
        /// Reference: Varsity Project by Christine Bittle - Player Data Controllers
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "Applicationdata/DeleteApplication/" + id;
            //Post as empty body content
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //Get 200 status code(OK)            
            if (response.IsSuccessStatusCode)
            {
                //Redirect to the List Page
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
        public ActionResult Error()
        {
            return View();
        }

    }
}