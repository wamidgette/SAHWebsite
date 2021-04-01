using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;


namespace SAH.Models
{
    public class IdentityModels : IdentityUser
    {
        public class ApplicationUser : IdentityUser
        {

        }

        public class SAHDataContext : IdentityDbContext<ApplicationUser>
        {
            public SAHDataContext()
                : base("name=SAHDataContext", throwIfV1Schema: false)
            {
            }

            public static SAHDataContext Create()
            {
                return new SAHDataContext();
            }

            //Instruction to set the models as tables in our database.
            public DbSet<Department> Departments { get; set; }
            public DbSet<Faq> Faqs { get; set; }
           
            //To Run the database, use code-first migrations
            //https://www.entityframeworktutorial.net/code-first/code-based-migration-in-code-first.aspx
        }
    }
}