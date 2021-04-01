using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAH.Models
{
    public class Faq
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentID { get; set; } 
        public virtual Department Department { get; set; }
    }

    public class FaqDto
    {
        public int FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
