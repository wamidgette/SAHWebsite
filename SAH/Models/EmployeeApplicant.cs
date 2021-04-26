﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace SAH.Models
{
    public class EmployeeApplicant
    {
        [Key]
        public int EmployeeApplicantId { get; set; }

        //Applicant reason
        public string Reason { get; set; }

        //THe applicant will be a user
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        //Applicant will apply for course
        [ForeignKey("Courses")]
        public int CourseId { get; set; }
        public virtual Courses Courses { get; set; }

        //Applicant has a role
       // [ForeignKey("Role")]
        //public int RoleId { get; set; }
       // public virtual Role Role { get; set; }
    }
    public class EmployeeApplicantDto
    {
        [DisplayName("Registration Number")]
        public int EmployeeApplicantId { get; set; }

        //Applicant reason
        public string Reason { get; set; }
        //Applicant is a user
        public string Id { get; set; }

        //Applicant linked to a course
        public int CourseId { get; set; }

        //Applicant has a role
       // public int RoleId { get; set; }
    }
}
