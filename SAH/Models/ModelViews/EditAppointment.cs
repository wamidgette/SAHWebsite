using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAH.Models.ModelViews
{
    public class EditAppointment
    {
        //Information about the FAQ
        public AppointmentDto AppointmentDto { get; set; }

        public SelectList DepartmentsSelectList { get; set; }

        public SelectList DoctorsSelectList { get; set; }
    }
}