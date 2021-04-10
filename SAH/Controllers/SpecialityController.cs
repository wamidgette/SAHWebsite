using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SAH.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace SAH.Controllers
{
    public class SpecialityController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static SpecialityController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };

            client = new HttpClient(handler);
            Debug.WriteLine(handler);
            Debug.WriteLine(client);
            client.BaseAddress = new Uri("https://localhost:44378/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
        }

        // GET: Speciality/List
        [HttpGet]
        public ActionResult List()
        {
            string url = "specialitydata/getspecialities";
            Debug.WriteLine(client.GetAsync(url).Result);
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<SpecialityDto> SelectedSpecialities = response.Content.ReadAsAsync<IEnumerable<SpecialityDto>>().Result;
                return View(SelectedSpecialities);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Speciality/Details/5
        public ActionResult Details(int id)
        {
            string url = "specialitydata/findspeciality/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into speciality data transfer object
                SpecialityDto SelectedSpeciality = response.Content.ReadAsAsync<SpecialityDto>().Result;
                return View(SelectedSpeciality);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Speciality/Create
        public ActionResult Create()
        {
            SpecialityDto Speciality = new SpecialityDto();
            return View(Speciality);
        }

        // POST: Speciality/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Speciality SpecialityInfo)
        {
            //Debug.WriteLine(SpecialityInfo.SpecialityID);
            string url = "specialitydata/addspeciality";
            //Debug.WriteLine(jss.Serialize(SpecialityInfo));
            HttpContent content = new StringContent(jss.Serialize(SpecialityInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                int specialityid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = specialityid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Speciality/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "specialitydata/findspeciality/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into speciality data transfer object
                SpecialityDto SelectedSpeciality = response.Content.ReadAsAsync<SpecialityDto>().Result;
                return View(SelectedSpeciality);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Speciality/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Speciality SpecialityInfo)
        {
            //Debug.WriteLine(SpecialityInfo.SpecialityID);
            string url = "specialitydata/updatespeciality/" + id;
            //Debug.WriteLine(jss.Serialize(SpecialityInfo));
            HttpContent content = new StringContent(jss.Serialize(SpecialityInfo));
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

        // GET: Speciality/DeleteConfirm/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "specialitydata/findspeciality/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into speciality data transfer object
                SpecialityDto SelectedSpeciality = response.Content.ReadAsAsync<SpecialityDto>().Result;
                return View(SelectedSpeciality);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Speciality/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "specialitydata/deletespeciality/" + id;
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
