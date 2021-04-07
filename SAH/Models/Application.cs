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

        public string ApplicationHasFile { get; set; }

        public string FileExtension { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Job")]
        public int JobId { get; set; }
        public virtual Job Job { get; set; }

    }

    public class ApplicationDto
    {
        public int ApplicationId { get; set; }

        [DisplayName("Application Comments")]
        public string Comment { get; set; }

        [DisplayName("Application with File")]
        public string ApplicationHasFile { get; set; }
        
        public string FileExtension { get; set; }
        
        public string UserId { get; set; }

        
        public string JobId { get; set; }


    }







}