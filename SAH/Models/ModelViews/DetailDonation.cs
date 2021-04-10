using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class DetailDonation
    {
        public DonationDto Donation { get; set; }
        public UserDto User { get; set; }
        public DepartmentDto Department { get; set; }
    }
}