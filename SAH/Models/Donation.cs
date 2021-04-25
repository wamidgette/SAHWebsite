using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAH.Models
{
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }
        public decimal AmountOfDonation { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime DonationDate { get; set; }

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }

    public class DonationDto
    {
        [DisplayName("Donation ID")]
        public int DonationId { get; set; }

        [DisplayName("Amount of Donation")]
        [Required(ErrorMessage = "Please Enter a Donaion amount.")]
        public decimal AmountOfDonation { get; set; }

        [DisplayName("Payment Method")]
        [Required(ErrorMessage = "Please Select a Payment Method.")]
        public string PaymentMethod { get; set; }

        [DisplayName("Donation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime DonationDate { get; set; }
        public string Id { get; set; }
        [Required(ErrorMessage = "Please Select a Department Name.")]
        public int DepartmentId { get; set; }
    }
}