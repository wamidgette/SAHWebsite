﻿using System;
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
            List<ApplicationUser> Users = db.Users.ToList();

            List<ApplicationUserDto> UserDtos = new List<ApplicationUserDto> { };

            foreach (var User in Users)
            {
                ApplicationUserDto NewUser = new ApplicationUserDto

                {
                    Id = User.Id,
/*                    RoleId = User.RoleId,
*/                  FirstName = User.FirstName,
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

                UserDtos.Add(NewUser);
            }

            return Ok(UserDtos);
        }
        //Get: userdata/GetUserbyId
        /// <summary>
        /// Takes get request with parameter of type applicationuser id (string)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The user corresponding to the id given</returns>
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult GetUserById(string id)
        {

            // List of all the users
            ApplicationUser SelectedUser = db.Users.Where(u=>u.Id == id).First();

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

        [ResponseType(typeof(IEnumerable<UserDto>))]
        public IHttpActionResult GetUsersByRoleId(int id)
        {
            // List of all the users
            List<ApplicationUser> Users = db.Users.Where(u => u.Roles.Any(r => r.RoleId.Equals(id))).ToList(); ;

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

            return Ok(UserDtos);
        }


    }
}