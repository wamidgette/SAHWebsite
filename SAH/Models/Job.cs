using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAH.Models
{
    public class Job
    {
        //This class is about the application entity
        [Key]
        public int JobId { get; set; }
        public string Position { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Requirement { get; set; }
        public DateTime Deadline { get; set; }

        //A Job Position can have many Application
        public ICollection<Application> Applications { get; set; }
    }
        
        //Data Transfer Object from Job
        public class JobDto
        {
            [DisplayName("Job ID")]   
            public int JobId { get; set; }

            [DisplayName("Job Position")]
            public string Position { get; set; }
            
            public string Category { get; set; }

            [DisplayName("Job Type")]
            public string Type { get; set; }

            [DisplayName("Requirements")]
            public string Requirement { get; set; }

            [DisplayName("Deadline")]
            public DateTime Deadline { get; set; }


        }



    
}