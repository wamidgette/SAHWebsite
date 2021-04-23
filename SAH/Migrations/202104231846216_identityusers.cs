namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class identityusers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Chats", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Messages", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Courses", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.EmployeeApplicants", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Donations", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Tickets", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "SpecialityId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "DepartmentId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Phone", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "PostalCode", c => c.String());
            AddColumn("dbo.AspNetUsers", "EmployeeNumber", c => c.Int());
            AddColumn("dbo.AspNetUsers", "HealthCardNumber", c => c.String());
            AddColumn("dbo.AspNetUsers", "Gender", c => c.String());
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Applications", "ApplicationUser_Id");
            CreateIndex("dbo.Chats", "ApplicationUser_Id");
            CreateIndex("dbo.Messages", "ApplicationUser_Id");
            CreateIndex("dbo.Courses", "ApplicationUser_Id");
            CreateIndex("dbo.EmployeeApplicants", "ApplicationUser_Id");
            CreateIndex("dbo.Donations", "ApplicationUser_Id");
            CreateIndex("dbo.Tickets", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "SpecialityId");
            CreateIndex("dbo.AspNetUsers", "DepartmentId");
            AddForeignKey("dbo.Applications", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Chats", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Departments", "DepartmentId");
            AddForeignKey("dbo.Donations", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.EmployeeApplicants", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "SpecialityId", "dbo.Specialities", "SpecialityId");
            AddForeignKey("dbo.Tickets", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "SpecialityId", "dbo.Specialities");
            DropForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmployeeApplicants", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Donations", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chats", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Applications", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentId" });
            DropIndex("dbo.AspNetUsers", new[] { "SpecialityId" });
            DropIndex("dbo.Tickets", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Donations", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.EmployeeApplicants", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Messages", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Chats", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Applications", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "HealthCardNumber");
            DropColumn("dbo.AspNetUsers", "EmployeeNumber");
            DropColumn("dbo.AspNetUsers", "PostalCode");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "Phone");
            DropColumn("dbo.AspNetUsers", "DepartmentId");
            DropColumn("dbo.AspNetUsers", "SpecialityId");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.Tickets", "ApplicationUser_Id");
            DropColumn("dbo.Donations", "ApplicationUser_Id");
            DropColumn("dbo.EmployeeApplicants", "ApplicationUser_Id");
            DropColumn("dbo.Courses", "ApplicationUser_Id");
            DropColumn("dbo.Messages", "ApplicationUser_Id");
            DropColumn("dbo.Chats", "ApplicationUser_Id");
            DropColumn("dbo.Applications", "ApplicationUser_Id");
        }
    }
}
