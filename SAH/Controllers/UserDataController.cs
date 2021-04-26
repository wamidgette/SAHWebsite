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
using Microsoft.AspNet.Identity;


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

        [ResponseType(typeof(IEnumerable<ApplicationUserDto>))]
        public IHttpActionResult GetUsers()
        {

            // List of all the users
            List<ApplicationUser> ApplicationUsers = db.Users.ToList();

            List<ApplicationUserDto> ApplicationUserDtos = new List<ApplicationUserDto> { };

            foreach (var ApplicationUser in ApplicationUsers)
            {
                ApplicationUserDto NewUser = new ApplicationUserDto

                {
                    Id = ApplicationUser.Id,
                    /*                    RoleId = User.RoleId,
                    */
                    FirstName = ApplicationUser.FirstName,
                    LastName = ApplicationUser.LastName,
                    SpecialityId = ApplicationUser.SpecialityId,
                    DepartmentId = ApplicationUser.DepartmentId,
                    Email = ApplicationUser.Email,
                    PhoneNumber = ApplicationUser.PhoneNumber,
                    Address = ApplicationUser.Address,
                    PostalCode = ApplicationUser.PostalCode,
                    PasswordHash = ApplicationUser.PasswordHash,
                    UserName = ApplicationUser.UserName,
                    EmployeeNumber = ApplicationUser.EmployeeNumber,
                    HealthCardNumber = ApplicationUser.HealthCardNumber,
                    Gender = ApplicationUser.Gender,
                    DateOfBirth = ApplicationUser.DateOfBirth

                };

                ApplicationUserDtos.Add(NewUser);
            }

            return Ok(ApplicationUserDtos);
        }

        //Get: userdata/GetUserbyId
        /// <summary>
        /// Takes get request with parameter of type applicationuser id (string)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The user corresponding to the id given</returns>
        [HttpGet]
        [ResponseType(typeof(ApplicationUserDto))]
/*        [Authorize()]*/
        public IHttpActionResult GetUserById(string Id)
        {
            Debug.WriteLine("IN GET USER METHOD - USER ID " + Id);
            // List of all the users
            ApplicationUser SelectedUser = db.Users.Where(u=>u.Id == Id).First();

            Debug.WriteLine(SelectedUser.Id);

            if (SelectedUser == null)
            {
                Debug.WriteLine("COULD NOT FIND USER IN API");
                return NotFound();
            }

            List<ApplicationUserDto> UserDtos = new List<ApplicationUserDto> { };

            ApplicationUserDto User = new ApplicationUserDto
                {
                    Id = SelectedUser.Id,
                    /*                    RoleId = User.RoleId,
                    */
                    FirstName = SelectedUser.FirstName,
                    LastName = SelectedUser.LastName,
                    SpecialityId = SelectedUser.SpecialityId,
                    DepartmentId = SelectedUser.DepartmentId,
                    Email = SelectedUser.Email,
                    PhoneNumber = SelectedUser.PhoneNumber,
                    Address = SelectedUser.Address,
                    PostalCode = SelectedUser.PostalCode,
                    PasswordHash = SelectedUser.PasswordHash,
                    UserName = SelectedUser.UserName,
                    EmployeeNumber = SelectedUser.EmployeeNumber,
                    HealthCardNumber = SelectedUser.HealthCardNumber,
                    Gender = SelectedUser.Gender,
                    DateOfBirth = SelectedUser.DateOfBirth

                };
          

            return Ok(User);
        }



        [HttpGet]
        [ResponseType(typeof(IEnumerable<ApplicationUserDto>))]
/*        [Authorize()]*/
        public IHttpActionResult GetUsersByRoleId(string id)
        {
            Debug.WriteLine("YOU ARE IN THE GET USERS BY ROLE ID API METHOD");
            // List of all the users
            List<ApplicationUser> Users = db.Users.Where(u => u.Roles.Any(r => r.RoleId==id)).ToList(); ;

            List<ApplicationUserDto> UserDtos = new List<ApplicationUserDto> { };

            foreach (var User in Users)
            {
                ApplicationUserDto ThisUser = new ApplicationUserDto

                {
                    Id = User.Id,
                    /*                    RoleId = User.RoleId,
                    */
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    SpecialityId = User.SpecialityId,
                    DepartmentId = User.DepartmentId,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber,
                    Address = User.Address,
                    PostalCode = User.PostalCode,
                    PasswordHash = User.PasswordHash,
                    UserName = User.UserName,
                    EmployeeNumber = User.EmployeeNumber,
                    HealthCardNumber = User.HealthCardNumber,
                    Gender = User.Gender,
                    DateOfBirth = User.DateOfBirth

                };

                UserDtos.Add(ThisUser);
            }

            Debug.WriteLine("THISUSER APPLICATIONUSERDTO OBJECT: " + UserDtos);

            return Ok(UserDtos);
        }

        

    }
}