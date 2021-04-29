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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

            //The view needs to be sent a list of all the Departments so the client can select an Apointmenr for an appointmnet in the view
            modelView.DepartmentsSelectList = GetDepartmentSelectList();

            //The view needs to be sent a list of all the Doctors so the client can select a Doctors for appointmnet in the view
            modelView.DoctorsSelectList = GetDoctorsSelectList();

            return View(modelView);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
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

        // GET: Appointment/Create/5
        [HttpGet]
        public ActionResult Create()
        {
            //Model used to combine an appointmen object, departments list and doctors list for dropdowns
            EditAppointment modelView = new EditAppointment();

            // Appointment is empty for before creating new Appointment
            modelView.AppointmentDto = new AppointmentDto();

            //The view needs to be sent a list of all the Departments so the client can select an Apointment for an appointmnet in the view
            modelView.DepartmentsSelectList = GetDepartmentSelectList();

            //The view needs to be sent a list of all the Doctors so the client can select a Doctor for appointmnet in the view
            modelView.DoctorsSelectList = GetDoctorsSelectList();

            return View(modelView);
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(EditAppointment appointmentInfo)
        {
            string url = "appointmentdata/AddAppointment";

            HttpContent content = new StringContent(jss.Serialize(appointmentInfo.AppointmentDto));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: appointment/DeleteConfirm/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
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

        // POST: appointment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
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

        private SelectList GetDepartmentSelectList()
        {
            string url = "DepartmentData/GetDepartments/";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departmentsList = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            // Convert departmentsList to  SelectList
            SelectList departmentsSelectList = new SelectList(departmentsList, "DepartmentId", "DepartmentName");
            return departmentsSelectList;
        }

        private SelectList GetDoctorsSelectList()
        {
            string url = "User/GetDoctors/";
            //HttpResponseMessage response = client.GetAsync(url).Result;
            //IEnumerable<DepartmentDto> departmentsList = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            //// Convert departmentsList to  SelectList
            //SelectList departmentsSelectList = new SelectList(departmentsList, "DepartmentId", "DepartmentName");
            //return departmentsSelectList;
            return null;
        }
    }
}