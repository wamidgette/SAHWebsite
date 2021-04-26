using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace SAH.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [ForeignKey("Speciality")]
        public int? SpecialityId { get; set; }
        public virtual Speciality Speciality { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }        
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int? EmployeeNumber { get; set; }
        public string HealthCardNumber { get; set; }
        public string Gender { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        //A user can have many tickets
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<Donation> Donations { get; set; }
        public ICollection<Courses> Courses { get; set; }
        public ICollection<EmployeeApplicant> EmployeeApplicants { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Application> Applications { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //public static implicit operator ApplicationUser(ApplicationUser v)
        //{
        //    throw new NotImplementedException();
        //}
    }
     
    public class ApplicationUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public int? SpecialityId { get; set; }       
        
        public int? DepartmentId { get; set; }       
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int? EmployeeNumber { get; set; }
        public string HealthCardNumber { get; set; }
        public string Gender { get; set; }
        public System.DateTime DateOfBirth { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Id { get; set; }

        



    }

    public class SAHDataContext : IdentityDbContext<ApplicationUser>
    {
        public SAHDataContext()
            : base("SAHDataContext", throwIfV1Schema: false)
        {
        }

        public static SAHDataContext Create()
        {
            return new SAHDataContext();
        }

        //Instruction to set the models as tables in our database.
        public DbSet<Department> Departments { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        //public DbSet<User> OurUsers { get; set; }   //There was conflict with default user table
        //public DbSet<Role> OurRoles { get; set; }   //There was conflict with default role table
        public DbSet<ParkingSpot> Spots { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<EmployeeApplicant> EmployeeApplicant { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


    }
}