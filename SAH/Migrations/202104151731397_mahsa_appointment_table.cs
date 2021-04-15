namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mahsa_appointment_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Address = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        PostalCode = c.String(),
                        Email = c.String(),
                        HelthCardNumber = c.String(),
                        PhoneNumber = c.String(),
                        Note = c.String(),
                        PreferedTime = c.String(),
                        AppintmentDateTime = c.DateTime(nullable: false),
                        IsFirstTimeVisit = c.Boolean(nullable: false),
                        IsUrgent = c.Boolean(nullable: false),
                        UserID = c.Int(),
                        DepartmentID = c.Int(),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.DepartmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "UserID", "dbo.Users");
            DropForeignKey("dbo.Appointments", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.Appointments", new[] { "DepartmentID" });
            DropIndex("dbo.Appointments", new[] { "UserID" });
            DropTable("dbo.Appointments");
        }
    }
}
