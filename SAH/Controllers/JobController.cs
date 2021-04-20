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
    public class JobController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static JobController()
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

        // GET: Job/List
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers
        public ActionResult List()
        {
            string url = "JobData/GetJobs";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<JobDto> SelectedJobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;
                return View(SelectedJobs);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Job/Details/5
        /// Reference:Varsity Project by Christine Bittle - Team Data Controllers
        public ActionResult Details(int id)
        {
            //ShowJob, model view used
            ShowJob ModelView = new ShowJob();

            string url = "JobData/FindJob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Response 200 code if it is ok
            if (response.IsSuccessStatusCode)
            {
                JobDto SelectedJob = response.Content.ReadAsAsync<JobDto>().Result;
                ModelView.Job = SelectedJob;

                // Get Applications by Job Id
                url = "JobData/GetApplicationsForJob/" + id;
                response = client.GetAsync(url).Result;

                IEnumerable<ApplicationDto> SelectedApplications = response.Content.ReadAsAsync<IEnumerable<ApplicationDto>>().Result;
                ModelView.Applications = SelectedApplications;

                return View(ModelView);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Job/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Job/Create
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Job JobInfo)
        {
            Debug.WriteLine(JobInfo.Position);
            string url = "Jobdata/AddJob";
            Debug.WriteLine(jss.Serialize(JobInfo));
            HttpContent content = new StringContent(jss.Serialize(JobInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //Redirect to the Details Page
                int JobId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = JobId });
            }
            else
            {
                //Redirect to the Error Page if it an error occur or it is unsuccessful
                return RedirectToAction("Error");
            }
        }

        // GET: Job/Edit/5
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers
        public ActionResult Edit(int id)
        {
            //Get the selected Job from the database
            string url = "Jobdata/FindJob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Catch 200 status code(OK)

            if (response.IsSuccessStatusCode)
            {
                //Put data into Job data transfer object
                JobDto SelectedJob = response.Content.ReadAsAsync<JobDto>().Result;
                return View(SelectedJob);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Job/Edit/5
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Job JobInfo)
        {
            Debug.WriteLine(JobInfo.Position);
            string url = "Jobdata/UpdateJob/" + id;
            Debug.WriteLine(jss.Serialize(JobInfo));
            HttpContent content = new StringContent(jss.Serialize(JobInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                //Redirect to the details if it is successful
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                //Unsuscessful = error page
                return RedirectToAction("Error");
            }
        }

        // GET: Job/Delete/5
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            //Get requested Job from the database
            string url = "JobData/FindJob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Put data into job data transfer object
                JobDto SelectedJob = response.Content.ReadAsAsync<JobDto>().Result;
                return View(SelectedJob);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Job/Delete/5
        /// Reference: Varsity Project by Christine Bittle - Team Data Controllers
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "Jobdata/DeleteJob/" + id;
            //post as empty body content
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //Get 200 status code(OK) 
            if (response.IsSuccessStatusCode)
            {

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