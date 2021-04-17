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

        // GET: Faq/PublicList
        [HttpGet]
        public ActionResult PublicList()
        {
            string url = "FaqData/GetPublicFaqs";
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
            //Model used to combine a Parking Spot object and its tickets
            EditFaq ModelView = new EditFaq();

            //Get the current ParkingSpot object
            string url = "FaqData/FindFaq/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                FaqDto SelectedFaq = response.Content.ReadAsAsync<FaqDto>().Result;
                ModelView.Faq = SelectedFaq;
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
                ModelView.DepartmentsSelectList = departmentsSelectList;
            }
            else
            {
                return RedirectToAction("Error");
            }

            return View(ModelView);
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

        public ActionResult Error()
        {
            return View();
        }
    }
}