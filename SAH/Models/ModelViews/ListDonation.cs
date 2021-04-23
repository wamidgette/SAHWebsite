using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAH.Models.ModelViews
{
    public class ListDonation
    {
        public bool IsAdmin { get; set; }

        public IEnumerable<DonationDto> AllDonations { get; set; }
        public IEnumerable<UserDto> AllUsers { get; set; }
        public IEnumerable<DepartmentDto> AllDepartments { get; set; }
        
        [DisplayName("Donation ID")]
        public int DonationId { get; set; }
        [DisplayName("Amount Of Donation")]
        public decimal AmountOfDonation { get; set; }
        public string PaymentMethod { get; set; }
        [Column(TypeName = "DateTime2")]
        [DisplayName("Donation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime DonationDate { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        [DisplayName("Donee Name")]
        public string DepartmentName { get; set; }
        
    }

}
