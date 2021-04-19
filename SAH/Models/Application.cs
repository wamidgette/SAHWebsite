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


        //An application one user
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        //An application per Job

        [ForeignKey("Job")]
        public int JobId { get; set; }
        public virtual Job Job { get; set; }

    }

    public class ApplicationDto
    {
        public int ApplicationId { get; set; }

        [DisplayName("Application Comments")]
        public string Comment { get; set; }

        [DisplayName("Including File")]
                
        public int UserId { get; set; }

        
        public int JobId { get; set; }


    }







}