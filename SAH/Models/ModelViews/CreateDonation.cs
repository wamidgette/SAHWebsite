using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class CreateDonation
    {
        public DonationDto Donation { get; set; }
        public IEnumerable<DepartmentDto> AllDepartments { get; set; }
        public UserDto User { get; set; }
    }
}