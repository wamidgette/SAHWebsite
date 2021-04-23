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

namespace SAH.Controllers
{
    public class DonationController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static DonationController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //Cookies are manually set in RequestHeader
                //UseCookies = false
            };

            client = new HttpClient(handler);
            Debug.WriteLine(handler);
            Debug.WriteLine(client);
            client.BaseAddress = new Uri("https://localhost:44378/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
        }

        /// <summary>
        /// Get the authentication credentials from the cookies.
        /// Remember this is not considered a proper authentication technique for the webAPI
        /// </summary>
       private void GetApplicationCookie()
        {
            string token = "";
            //Reset cookies in HTTP client before get a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //Collect token and pass it to the WebAPI
            Debug.WriteLine("Token submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);
            
            return;
        }

        // GET: Donation/List
        public ActionResult List()
        {
            ListDonation ModelView = new ListDonation();
            ModelView.IsAdmin = User.IsInRole("Admin");

            //Get all donation related info
            string url = "donationdata/getdonationsinfo";
            //Debug.WriteLine(client.GetAsync(url).Result);
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ListDonation> SelectedInfo = response.Content.ReadAsAsync<IEnumerable<ListDonation>>().Result;
                Debug.WriteLine(SelectedInfo);

                return View(SelectedInfo);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Donation/Details/5
        public ActionResult Details(int id)
        {
            DetailDonation ModelView = new DetailDonation();

            //Get donation data
            string url = "donationdata/finddonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                //Put data into department data transfer object
                DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;
                ModelView.Donation = SelectedDonation;

                //Get donor data
                string urlDonor = "donationdata/finddonorfordonation/" + id;
                response = client.GetAsync(urlDonor).Result;
                UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ModelView.User = selectedUser;

                //Get department data
                string urlDep = "departmentdata/finddepartmentfordonation/" + id;
                response = client.GetAsync(urlDep).Result;
                DepartmentDto selectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                ModelView.Department = selectedDepartment;


                return View(ModelView);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Donation/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            CreateDonation ModelView = new CreateDonation();
            ModelView.Donation = new DonationDto();

            //Get department data
            string urlDep = "departmentdata/getdepartments";
            HttpResponseMessage responseDepartment = client.GetAsync(urlDep).Result;
            IEnumerable<DepartmentDto> PotentialDepartments = responseDepartment.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ModelView.AllDepartments = PotentialDepartments;

            return View(ModelView);
        }

        // POST: Donation/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Donation DonationInfo)
        {
            //Pass along authentication credential in http request
            GetApplicationCookie();

            //Debug.WriteLine(DonationInfo.DonationId);
            string url = "donationdata/adddonation";
            //Debug.WriteLine(jss.Serialize(DonationtInfo));
            HttpContent content = new StringContent(jss.Serialize(DonationInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                int donationid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = donationid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Donation/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            CreateDonation ModelView = new CreateDonation();

            //Get donation data
            string url = "donationdata/finddonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                //Put data into donation data transfer object
                DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;
                ModelView.Donation = SelectedDonation;

                //Get department data
                string urlDep = "departmentdata/getdepartments/";
                response = client.GetAsync(urlDep).Result;
                IEnumerable<DepartmentDto> allDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                ModelView.AllDepartments = allDepartments;

                //Get donor data
                string urlDonor = "donationdata/finddonorfordonation/" + id;
                response = client.GetAsync(urlDonor).Result;
                UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ModelView.User = selectedUser;

                return View(ModelView);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Donation/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Donation DonationInfo)
        {
            //Pass along authentication credential in http request
            GetApplicationCookie();

            //Debug.WriteLine(DonationInfo.DonationId);
            string url = "donationdata/updatedonation/" + id;
            Debug.WriteLine(jss.Serialize(DonationInfo));
            HttpContent content = new StringContent(jss.Serialize(DonationInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Debug.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Donation/DeleteConfirm/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            DetailDonation ModelView = new DetailDonation();

            //Get donation data
            string url = "donationdata/finddonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                //Put data into department data transfer object
                DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;
                ModelView.Donation = SelectedDonation;

                //Get donor data
                string urlDonor = "donationdata/finddonorfordonation/" + id;
                response = client.GetAsync(urlDonor).Result;
                UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ModelView.User = selectedUser;

                //Get department data
                string urlDep = "donationdata/finddepartmentfordonation/" + id;
                response = client.GetAsync(urlDep).Result;
                DepartmentDto selectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                ModelView.Department = selectedDepartment;


                return View(ModelView);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Donation/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            //Pass along authentication credential in http request
            GetApplicationCookie();

            string url = "donationdata/deletedonation/" + id;
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
