using SAH.Models;
using SAH.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SAH.Controllers
{
    public class AppointmentController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        /// <summary>
        /// This allows us to access a pre-defined C# HttpClient 'client' variable for sending POST and GET requests to the data access layer.
        /// </summary>
        static AppointmentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44378/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Appointment/List
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public ActionResult List()
        {
             string url = "AppointmentData/GetAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<AppointmentDto> appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
                return View(appointments);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointment/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EditAppointment modelView = new EditAppointment();

          //  AppointmentDto appointmentDto = new AppointmentDto();

            //Get the current Appointment object
            string url = "AppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                AppointmentDto selectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                modelView.AppointmentDto = selectedAppointment;
            }
            else
            {
                return RedirectToAction("Error");
            }

            url = "DepartmentData/GetDepartments/";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DepartmentDto> departmentsList = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                // Convert departmentsList to  SelectList
                SelectList departmentsSelectList = new SelectList(departmentsList, "DepartmentId", "DepartmentName");
                modelView.DepartmentsSelectList = departmentsSelectList;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //url = "UserData/GetDoctors/";
            //response = client.GetAsync(url).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    IEnumerable<UserDto> doctors = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            //    modelView.DoctorList = doctors;
            //}
            //else
            //{
            //    return RedirectToAction("Error");
            //}


            return View(modelView);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        //[Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, EditAppointment modelView)
        {
            string url = "appointmentdata/updateappointment/" + id;

            HttpContent content = new StringContent(jss.Serialize(modelView.AppointmentDto));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("update Appointment request succeeded");
                return RedirectToAction("List");
            }
            else
            {
                Debug.WriteLine("update Appointment request failed with error: " + response.StatusCode.ToString());
                return RedirectToAction("Error");
            }
        }

        // GET: appointment/DeleteConfirm/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            //Get the current ParkingSpot object
            string url = "appointmentdata/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                AppointmentDto appointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                return View(appointment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: faq/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "appointmentdata/deleteappointment/" + id;
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