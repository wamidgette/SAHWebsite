﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAH.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string HelthCardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public string PreferedTime { get; set; }
        public DateTime AppintmentDateTime { get; set; }
        public bool IsFirstTimeVisit { get; set; }
        public bool IsUrgent { get; set; }

        //Doctor
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentID { get; set; }
        public virtual Department Department { get; set; }
    }

    public class AppointmentDto
    {
        [DisplayName("Appointment ID")]
        public int AppointmentID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string HelthCardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public string PreferedTime { get; set; }
        public DateTime AppintmentDateTime { get; set; }

        [DisplayName("First Time Visit")]
        public bool IsFirstTimeVisit { get; set; }

        [DisplayName("Urgent")]
        public bool IsUrgent { get; set; }

        [DisplayName("Department ID")]
        public int? DepartmentID { get; set; }

        [DisplayName("Department")]
        public string DepartmentName { get; set; }

        [DisplayName("Doctor ID")]
        public int? UserId { get; set; }

        [DisplayName("Doctor")]
        public string DoctorName { get; set; }

    }
}