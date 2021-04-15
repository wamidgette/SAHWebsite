using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAH.Models
{
    public class Faq
    {
        [Key]
        public int FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Publish { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentID { get; set; } 
        public virtual Department Department { get; set; }
    }

    public class FaqDto
    {
        [Key]
        [DisplayName("FAQ ID")]
        public int FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Publish { get; set; }
        public int? DepartmentID { get; set; }

    }
}
