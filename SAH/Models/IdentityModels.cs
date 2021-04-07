using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SAH.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
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
        public DbSet<User> OurUsers { get; set; }   //There was conflict with default user table
        public DbSet<Role> OurRoles { get; set; }   //There was conflict with default role table
        public DbSet<ParkingSpot> Spots { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Speciality> Specialties { get; set; }
        public DbSet<Donation> Donations { get; set; }
  
    }
}