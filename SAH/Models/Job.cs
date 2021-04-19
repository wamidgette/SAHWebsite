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
        [Key]
        public int JobId { get; set; }
        public string Position { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Requirement { get; set; }
        public DateTime Deadline { get; set; }

        //A JobPosition can have many application
        public ICollection<Application> Applications { get; set; }

    }

        public class JobDto
        {
            public int JobId { get; set; }

            [DisplayName("Job Position")]
            public string Position { get; set; }
            
            public string Category { get; set; }

            [DisplayName("Type of Job")]
            public string Type { get; set; }

            [DisplayName("Job Requirements")]
            public string Requirement { get; set; }

            [DisplayName("Deadline")]
            public DateTime Deadline { get; set; }


        }



    
}