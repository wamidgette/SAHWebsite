using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAH.Models.ModelViews
{
    public class ListDonation
    {
        public IEnumerable<DonationDto> AllDonations { get; set; }
        public IEnumerable<UserDto> AllUsers { get; set; }
        public IEnumerable<DepartmentDto> AllDepartments { get; set; }
    }
}