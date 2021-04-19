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

            client.BaseAddress = new Uri("https://localhost:44378/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));



        }
        // GET: Application/List
        public ActionResult List()
        {
            string url = "EmployeeApplicantData/GetAllApplications";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ShowEmployeeApplicant> SelectedApplications = response.Content.ReadAsAsync<IEnumerable<ShowEmployeeApplicant>>().Result;
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
            ShowEmployeeApplicant ShowEmployeeApplicant = new ShowEmployeeApplicant();

            //Find the application from the database
            string url = "EmployeeApplicantData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                EmployeeApplicantDto SelectedApplication = response.Content.ReadAsAsync<EmployeeApplicantDto>().Result;
                ShowEmployeeApplicant.EmployeeApplicant = SelectedApplication;

                //Associated Application with User
                url = "EmployeeApplicantData/GetApplicationUser/" + id;
                response = client.GetAsync(url).Result;
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ShowEmployeeApplicant.User = SelectedUser;

                //Associated application with Job
                url = "EmployeeApplicantData/GetApplicationJob/" + id;
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

        // GET: Application/Create
        public ActionResult Create()
        {
            //Get all the users for dropdown list
            EditEmployeeApplicant EditEmployeeApplicant = new EditEmployeeApplicant();
            string url = "EmployeeApplicantData/GetUsers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<UserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            EditEmployeeApplicant.AllUsers = SelectedUsers;

            //Get all the jobs for dropdown list
            url = "EmployeeApplicantData/GetCourses";
            response = client.GetAsync(url).Result;

            IEnumerable<CoursesDto> SelectedCourses = response.Content.ReadAsAsync<IEnumerable<CoursesDto>>().Result;
            EditEmployeeApplicant.AllCourses = SelectedCourses;

            return View(EditEmployeeApplicant);
        }

        // POST: Application/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeApplicant EmployeeApplicant)
        {
            //Add a new application to the database
            string url = "EmployeeApplicantData/AddApplication";
            HttpContent content = new StringContent(jss.Serialize(EmployeeApplicant));

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
            EditEmployeeApplicant NewEmployeeApplicant = new EditEmployeeApplicant();

            //Get the selected ticket from the database
            string url = "EmployeeApplicantData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                EmployeeApplicantDto SelectedApplication = response.Content.ReadAsAsync<EmployeeApplicantDto>().Result;
                NewEmployeeApplicant.EmployeeApplicant = SelectedApplication;
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
                NewEmployeeApplicant.AllUsers = SelectedUsers;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //Get all jobs from the database for dropdown list
            url = "EmployeeApplicantData/GetCourses";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<CoursesDto> SelectedJobs = response.Content.ReadAsAsync<IEnumerable<CoursesDto>>().Result;
                NewEmployeeApplicant.AllCourses = SelectedJobs;
            }
            else
            {
                return RedirectToAction("Error");
            }
            return View(NewEmployeeApplicant);

        }

        // POST: Application/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeApplicant EmployeeApplicant)
        {
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

        // GET: Application/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            //Get current ticket from the database
            string url = "EmployeeApplicantData/FindApplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                EmployeeApplicantDto SelectedApplication = response.Content.ReadAsAsync<EmployeeApplicantDto>().Result;
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
            string url = "EmployeeApplicantData/DeleteApplication/" + id;
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
