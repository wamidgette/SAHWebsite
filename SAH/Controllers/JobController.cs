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

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static JobController()
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

        // GET: Job/List
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
        public ActionResult Details(int id)
        {
            //Model used to combine a Parking Spot object and its tickets
            ShowJob ModelViews = new ShowJob();

            //Get the current ParkingSpot object
            string url = "JobData/FindJob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                JobDto SelectedJob = response.Content.ReadAsAsync<JobDto>().Result;
                ModelViews.Job = SelectedJob; 
                
            }
            else
            {
                return RedirectToAction("Error");
            }

            url = "JobData/GetJobApplications/" + id;
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {              

                //No an error because a Job not having any applicants is not a problem
               
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<ApplicationDto> SelectedApplications = response.Content.ReadAsAsync<IEnumerable<ApplicationDto>>().Result;
                ModelViews.Applications = SelectedApplications;
            }
            else
            {
                return RedirectToAction("Error");
            }


            return View(ModelViews);
        }

        // GET: Job/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Job/Create
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

                int JobId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = JobId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Job/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Jobdata/FindJob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
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
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Job/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "JobData/FindJob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
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

        // POST: Job/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "Jobdata/DeleteJob/" + id;
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

        public ActionResult Error()
        {
            return View();
        }

    }
}

