﻿using System;
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
using Microsoft.AspNet.Identity;

namespace SAH.Controllers
{
    public class CoursesController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static CoursesController()
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

        
        /// Grabs the authentication credentials which are sent to the Controller.
        
        private void GetApplicationCookie()
        {
            string token = "";
            
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Courses/List
        public ActionResult List()
        {
            ListCourses ModelViews = new ListCourses();
            ModelViews.isadmin = User.IsInRole("admin");

            string url = "CoursesData/GetCourses";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<CoursesDto> SelectedCourse = response.Content.ReadAsAsync<IEnumerable<CoursesDto>>().Result;
                return View(SelectedCourse);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Courses/Details/5
        public ActionResult Details(int id)
        {            
            //Model used to combine a Parking Spot object and its tickets
            ShowCourses ModelViews = new ShowCourses();
            ModelViews.isadmin = User.IsInRole("admin");

            //Get the current ParkingSpot object
            string url = "CoursesData/FindCourse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                CoursesDto SelectedCourse = response.Content.ReadAsAsync<CoursesDto>().Result;
                ModelViews.Courses = SelectedCourse;

            }
            else
            {
                return RedirectToAction("Error");
            }

            url = "CoursesData/GetCourseApplications/" + id;
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {

                //No an error because a Course not having any applicants is not a problem

                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<EmployeeApplicantDto> SelectedApplications = response.Content.ReadAsAsync<IEnumerable<EmployeeApplicantDto>>().Result;
                ModelViews.EmployeeApplications = SelectedApplications;
            }
            else
            {
                return RedirectToAction("Error");
            }


            return View(ModelViews);
        }

        // GET: Courses/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Courses CoursesInfo)
        {
            //pass authorization up to the data access layer
            GetApplicationCookie();

            Debug.WriteLine(CoursesInfo.CourseName);
            string url = "Coursesdata/AddCourse";
            Debug.WriteLine(jss.Serialize(CoursesInfo));
            HttpContent content = new StringContent(jss.Serialize(CoursesInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int CourseId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = CourseId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            string url = "Coursesdata/FindCourse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Course data transfer object
                CoursesDto SelectedCourse = response.Content.ReadAsAsync<CoursesDto>().Result;
                return View(SelectedCourse);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, Courses CoursesInfo)
        {
            Debug.WriteLine(CoursesInfo.CourseName);
            string url = "Coursesdata/UpdateCourse/" + id;
            Debug.WriteLine(jss.Serialize(CoursesInfo));
            HttpContent content = new StringContent(jss.Serialize(CoursesInfo));
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

        // GET: Courses/Delete/5
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CoursesData/FindCourse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Course data transfer object
                CoursesDto SelectedCourse = response.Content.ReadAsAsync<CoursesDto>().Result;
                return View(SelectedCourse);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            //pass authorization up to the data access layer
            GetApplicationCookie();

            string url = "Coursesdata/DeleteCourse/" + id;
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

