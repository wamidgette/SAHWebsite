﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAH.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        //A user has a role
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? SpecialityId { get; set; }
        public int? DepartmentId { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string PasswordHash { get; set; }
        public string Username { get; set; }
        public int? EmployeeNumber { get; set; }
        public string HealthCardNumber { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        //A user can have many tickets
        public ICollection<Ticket> Tickets { get; set; }


    }

    public class UserDto
    {
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? SpecialityId { get; set; }
        public int? DepartmentId { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string PasswordHash { get; set; }
        public string Username { get; set; }
        public int? EmployeeNumber { get; set; }
        public string HealthCardNumber { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}