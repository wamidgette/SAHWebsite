using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SAH.Models;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace SAH.Controllers
{
    public class UserDataController : ApiController
    {
        //The access to the SAH Project Database
        private SAHDataContext db = new SAHDataContext();

        /// <summary>
        /// Get request from all the Users in the Database
        /// </summary>
        /// <returns>User's List including userid, roleid, email, phone, address, etc...</returns>
        /// <example>
        /// GET: api/UserData/GetUsers
        /// </example>
        /// Reference: Varsity Project by Christine Bittle
        /// Code was scaffolded and adjusted

        [ResponseType(typeof(IEnumerable<UserDto>))]
        public IHttpActionResult GetUsers()
        {

            // List of all the users
            List<User> Users = db.OurUsers.ToList();

            List<UserDto> UserDtos = new List<UserDto> { };

            foreach (var User in Users)
            {
                UserDto NewUser = new UserDto

                {
                    UserId = User.UserId,
                    RoleId = User.RoleId,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    SpecialityId = User.SpecialityId,
                    DepartmentId = User.DepartmentId,
                    Email = User.Email,
                    Phone = User.Phone,
                    Address = User.Address,
                    PostalCode = User.PostalCode,
                    PasswordHash = User.PasswordHash,
                    Username = User.Username,
                    EmployeeNumber = User.EmployeeNumber,
                    HealthCardNumber = User.HealthCardNumber,
                    Gender = User.Gender,
                    DateOfBirth = User.DateOfBirth

                };

                UserDtos.Add(NewUser);
            }

            return Ok(UserDtos);
        }



    }
}