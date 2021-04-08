using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAH.Controllers
{
    public class EmployeeApplicantController : Controller
    {
        // GET: EmployeeApplicant
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmployeeApplicant/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeApplicant/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeApplicant/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeApplicant/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeApplicant/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeApplicant/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeApplicant/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
