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
    public class FaqController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        /// <summary>
        /// This allows us to access a pre-defined C# HttpClient 'client' variable for sending POST and GET requests to the data access layer.
        /// </summary>
        static FaqController()
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

        // GET: Faq/AdminList
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public ActionResult AdminList()
        {
            string url = "FaqData/GetFaqs";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<FaqDto> faqs = response.Content.ReadAsAsync<IEnumerable<FaqDto>>().Result;
                return View(faqs);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Faq/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //Model used to combine a faq object and departments list for dropdown
            EditFaq modelView = new EditFaq();

            //Get the current ParkingSpot object
            string url = "FaqData/FindFaq/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                FaqDto SelectedFaq = response.Content.ReadAsAsync<FaqDto>().Result;
                modelView.Faq = SelectedFaq;
            }
            else
            {
                return RedirectToAction("Error");
            }

            //The view needs to be sent a list of all the Departments so the client can select a Department for FAQ in the view
            modelView.DepartmentsSelectList = GetDepartmentSelectList();

            return View(modelView);
        }

        // POST: Faq/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        //[Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, EditFaq faqInfo)
        {
            string url = "faqdata/updatefaq/" + id;

            HttpContent content = new StringContent(jss.Serialize(faqInfo.Faq));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("update FAQ request succeeded");
                return RedirectToAction("AdminList");
            }
            else
            {
                Debug.WriteLine("update FAQ request failed with error: " + response.StatusCode.ToString());
                return RedirectToAction("Error");
            }
        }

        // GET: Faq/Create
        [HttpGet]
        public ActionResult Create()
        {
            //Model used to combine a Parking Spot object and its tickets
            EditFaq ModelView = new EditFaq();

            // FAQ is empty for before creating new FAQ
            ModelView.Faq = new FaqDto();

            //The view needs to be sent a list of all the Departments so the client can select a Department for FAQ in the view
            ModelView.DepartmentsSelectList = GetDepartmentSelectList();

            return View(ModelView);
        }

        // POST: Faq/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(EditFaq faqInfo)
        {
            string url = "faqdata/AddFaq";

            HttpContent content = new StringContent(jss.Serialize(faqInfo.Faq));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Add FAQ request succeeded");
                return RedirectToAction("AdminList");
            }
            else
            {
                Debug.WriteLine("Add FAQ request failed with error: " + response.StatusCode.ToString());
                return RedirectToAction("Error");
            }
        }

        // GET: faq/DeleteConfirm/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            //Get the current ParkingSpot object
            string url = "faqdata/FindFaq/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                FaqDto SelectedFaq = response.Content.ReadAsAsync<FaqDto>().Result;
                return View(SelectedFaq);
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
            string url = "faqdata/deletefaq/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdminList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Faq/PublicView
        [HttpGet]
        public ActionResult PublicView()
        {
            //Model used to combine a the faq list and the objects for the form 
            ViewFaq modelView = new ViewFaq();

            string url = "FaqData/GetPublicFaqs";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<FaqDto> faqs = response.Content.ReadAsAsync<IEnumerable<FaqDto>>().Result;
                modelView.FaqList = faqs;
            }
            else
            {
                return RedirectToAction("Error");
            }

            // FAQ is empty for before creating new FAQ
            modelView.newFaq = new FaqDto();

            //The view needs to be sent a list of all the Departments so the client can select a Department for FAQ in the view
            modelView.DepartmentsSelectList = GetDepartmentSelectList();

            return View(modelView);
        }

        // POST: Faq/PublicView
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult PublicView(ViewFaq faqInfo)
        {
            string url = "faqdata/AddFaq";

            HttpContent content = new StringContent(jss.Serialize(faqInfo.newFaq));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Add FAQ request succeeded");
                return RedirectToAction("PublicView");
            }
            else
            {
                Debug.WriteLine("Add FAQ request failed with error: " + response.StatusCode.ToString());
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
    }
}