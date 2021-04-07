using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAH.Models
{
    public class Speciality
    {
        [Key]
        public int SpecialityId { get; set; }
        public string SpecialityName { get; set; }
    }

    public class SpecialityDto
    {
        [DisplayName("Speciality ID")]
        public int SpecialityId { get; set; }
        [DisplayName("Speciality Name")]
        public string SpecialityName { get; set; }
    }
}