using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SAH.Models;
using SAH.Models.ModelViews;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace SAH.Controllers
{
    public class DepartmentController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static DepartmentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //Cookies are manually set in RequestHeader
                UseCookies = false
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

        // GET: Department/List
        public ActionResult List()
        {
            string url = "departmentdata/getdepartments";
            Debug.WriteLine(client);
            Debug.WriteLine(client.GetAsync(url));
            Debug.WriteLine(client.GetAsync(url).Result);
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DepartmentDto> SelectedDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                return View(SelectedDepartments);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            DetailDepartment ModelView = new DetailDepartment();

            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            
            if (response.IsSuccessStatusCode)
            {
                //Put data into department data transfer object
                //Department data
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                ModelView.Department = SelectedDepartment;

                //Get associated user data
                string urlDonor = "departmentdata/finddonorsfordepartment/" + id;
                response = client.GetAsync(urlDonor).Result;
                IEnumerable<ApplicationUserDto> selectedUser = response.Content.ReadAsAsync<IEnumerable<ApplicationUserDto>>().Result;
                ModelView.Users = selectedUser;

                return View(ModelView);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            DepartmentDto Department = new DepartmentDto();
            return View(Department);
        }

        // POST: Department/Create
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Department DepartmentInfo)
        {
            //Pass along authentication credential in http request
            GetApplicationCookie();

            //Debug.WriteLine(DepartmentInfo.DepartmentID);
            string url = "departmentdata/adddepartment";
            //Debug.WriteLine(jss.Serialize(DepartmentInfo));
            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                int departmentid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = departmentid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into department data transfer object
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(SelectedDepartment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Department/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Department DepartmentInfo)
        {

            //Pass along authentication credential in http request
            GetApplicationCookie();

            //Debug.WriteLine(DepartmentInfo.DepartmentID);
            string url = "departmentdata/updatedepartment/" + id;
            //Debug.WriteLine(jss.Serialize(DepartmentInfo));
            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Department/DeleteConfirm/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into department data transfer object
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(SelectedDepartment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Department/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            //Pass along authentication credential in http request
            GetApplicationCookie();

            string url = "departmentdata/deletedepartment/" + id;
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
