using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAH.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        public string Comment { get; set; }      


        //One application one user
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        //One application per Job

        [ForeignKey("Job")]
        public int JobId { get; set; }
        public virtual Job Job { get; set; }

    }

    //Data Transfer Object from Application
    public class ApplicationDto
    {
        [DisplayName("Application ID")]
        public int ApplicationId { get; set; }

        [DisplayName("Comments")]
        public string Comment { get; set; }

        [DisplayName("User ID")]                
        public int UserId { get; set; }

        [DisplayName("Job ID")]
        public int JobId { get; set; }


    }




}