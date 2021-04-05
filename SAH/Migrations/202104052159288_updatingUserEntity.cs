namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingUserEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "SpecialityId", c => c.Int());
            AddColumn("dbo.Users", "DepartmentId", c => c.Int());
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "Phone", c => c.Int());
            AddColumn("dbo.Users", "Address", c => c.String());
            AddColumn("dbo.Users", "PostalCode", c => c.String());
            AddColumn("dbo.Users", "PasswordHash", c => c.String());
            AddColumn("dbo.Users", "Username", c => c.String());
            AddColumn("dbo.Users", "EmployeeNumber", c => c.Int());
            AddColumn("dbo.Users", "HealthCardNumber", c => c.String());
            AddColumn("dbo.Users", "Gender", c => c.String());
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Speciality");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Speciality", c => c.String());
            DropColumn("dbo.Users", "DateOfBirth");
            DropColumn("dbo.Users", "Gender");
            DropColumn("dbo.Users", "HealthCardNumber");
            DropColumn("dbo.Users", "EmployeeNumber");
            DropColumn("dbo.Users", "Username");
            DropColumn("dbo.Users", "PasswordHash");
            DropColumn("dbo.Users", "PostalCode");
            DropColumn("dbo.Users", "Address");
            DropColumn("dbo.Users", "Phone");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "DepartmentId");
            DropColumn("dbo.Users", "SpecialityId");
        }
    }
}
