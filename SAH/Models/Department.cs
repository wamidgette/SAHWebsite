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
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Donation> Donations { get; set; }
    }


    public class DepartmentDto
    {
        [DisplayName("Department ID")]
        public int DepartmentId { get; set; }
        [DisplayName("Department Name")]
        [Required(ErrorMessage = "Please Enter a Department Name.")]
        public string DepartmentName { get; set; }
    }
}
