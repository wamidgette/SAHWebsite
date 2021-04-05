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
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }

    public class DonationDto
    {
        [DisplayName("Donation ID")]
        public int DonationId { get; set; }

        [DisplayName("Amount of Donation")]
        public decimal AmountOfDonation { get; set; }
        
        [DisplayName("Payment Method")]
        public string PaymentMethod { get; set; }

        [Column(TypeName = "DateTime2")]
        [DisplayName("Donation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime DonationDate { get; set; }
        public int? UserId { get; set; }
    }
}