using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAH.Models.ModelViews
{
    public class DetailDepartment
    {
        public DepartmentDto Department { get; set; }

        public IEnumerable<UserDto> Users { get; set; }
             
    }
}