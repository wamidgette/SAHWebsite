namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appliUserdiarra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        Id = c.String(maxLength: 128),
                        JobId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ApplicationId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.JobId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        SpecialityId = c.Int(),
                        DepartmentId = c.Int(),
                        Address = c.String(),
                        PostalCode = c.String(),
                        EmployeeNumber = c.Int(),
                        HealthCardNumber = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Specialities", t => t.SpecialityId)
                .Index(t => t.SpecialityId)
                .Index(t => t.DepartmentId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        ChatId = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ChatId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        SenderId = c.String(maxLength: 128),
                        ChatId = c.Int(nullable: false),
                        DateSent = c.DateTime(nullable: false),
                        Content = c.String(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.SenderId)
                .Index(t => t.ChatId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseCode = c.String(),
                        CourseName = c.String(),
                        StartOn = c.DateTime(nullable: false),
                        User_UserId = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.User_UserId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.EmployeeApplicants",
                c => new
                    {
                        EmployeeApplicantId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EmployeeApplicantId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.UserId)
                .Index(t => t.CourseId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        SpecialityId = c.Int(),
                        DepartmentId = c.Int(),
                        Email = c.String(),
                        Phone = c.Int(),
                        Address = c.String(),
                        PostalCode = c.String(),
                        PasswordHash = c.String(),
                        Username = c.String(),
                        EmployeeNumber = c.Int(),
                        HealthCardNumber = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Specialities", t => t.SpecialityId)
                .Index(t => t.RoleId)
                .Index(t => t.SpecialityId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        DonationId = c.Int(nullable: false, identity: true),
                        AmountOfDonation = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMethod = c.String(),
                        DonationDate = c.DateTime(nullable: false),
                        Id = c.String(maxLength: 128),
                        DepartmentId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.DonationId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.Id)
                .Index(t => t.DepartmentId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Specialities",
                c => new
                    {
                        SpecialityId = c.Int(nullable: false, identity: true),
                        SpecialityName = c.String(),
                    })
                .PrimaryKey(t => t.SpecialityId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        NumberPlate = c.String(),
                        EntryTime = c.DateTime(nullable: false),
                        Duration = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fees = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id = c.String(maxLength: 128),
                        SpotId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TicketId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.ParkingSpots", t => t.SpotId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.SpotId);
            
            CreateTable(
                "dbo.ParkingSpots",
                c => new
                    {
                        SpotId = c.Int(nullable: false, identity: true),
                        Zone = c.String(),
                        SpotNumber = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SpotId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        Position = c.String(),
                        Category = c.String(),
                        Type = c.String(),
                        Requirement = c.String(),
                        Deadline = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobId);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(),
                        Address = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        PostalCode = c.String(),
                        Email = c.String(),
                        HelthCardNumber = c.String(),
                        PhoneNumber = c.String(),
                        Note = c.String(),
                        PreferedTime = c.String(),
                        AppintmentDateTime = c.DateTime(),
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
            
            CreateTable(
                "dbo.Faqs",
                c => new
                    {
                        FaqID = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                        Publish = c.Boolean(nullable: false),
                        DepartmentID = c.Int(),
                    })
                .PrimaryKey(t => t.FaqID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ChatApplicationUsers",
                c => new
                    {
                        Chat_ChatId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Chat_ChatId, t.ApplicationUser_Id })
                .ForeignKey("dbo.Chats", t => t.Chat_ChatId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Chat_ChatId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Faqs", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Appointments", "UserID", "dbo.Users");
            DropForeignKey("dbo.Appointments", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Applications", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.Applications", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "SpotId", "dbo.ParkingSpots");
            DropForeignKey("dbo.Tickets", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "SpecialityId", "dbo.Specialities");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmployeeApplicants", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmployeeApplicants", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "SpecialityId", "dbo.Specialities");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Messages", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Donations", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Donations", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Donations", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Chats", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Applications", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.EmployeeApplicants", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "ChatId", "dbo.Chats");
            DropForeignKey("dbo.ChatApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatApplicationUsers", "Chat_ChatId", "dbo.Chats");
            DropIndex("dbo.ChatApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ChatApplicationUsers", new[] { "Chat_ChatId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Faqs", new[] { "DepartmentID" });
            DropIndex("dbo.Appointments", new[] { "DepartmentID" });
            DropIndex("dbo.Appointments", new[] { "UserID" });
            DropIndex("dbo.Tickets", new[] { "SpotId" });
            DropIndex("dbo.Tickets", new[] { "Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Donations", new[] { "User_UserId" });
            DropIndex("dbo.Donations", new[] { "DepartmentId" });
            DropIndex("dbo.Donations", new[] { "Id" });
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.Users", new[] { "SpecialityId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.EmployeeApplicants", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.EmployeeApplicants", new[] { "CourseId" });
            DropIndex("dbo.EmployeeApplicants", new[] { "UserId" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "User_UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "User_UserId" });
            DropIndex("dbo.Messages", new[] { "ChatId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Chats", new[] { "User_UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentId" });
            DropIndex("dbo.AspNetUsers", new[] { "SpecialityId" });
            DropIndex("dbo.Applications", new[] { "User_UserId" });
            DropIndex("dbo.Applications", new[] { "JobId" });
            DropIndex("dbo.Applications", new[] { "Id" });
            DropTable("dbo.ChatApplicationUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Faqs");
            DropTable("dbo.Appointments");
            DropTable("dbo.Jobs");
            DropTable("dbo.ParkingSpots");
            DropTable("dbo.Tickets");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Specialities");
            DropTable("dbo.Roles");
            DropTable("dbo.Donations");
            DropTable("dbo.Departments");
            DropTable("dbo.Users");
            DropTable("dbo.EmployeeApplicants");
            DropTable("dbo.Courses");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Messages");
            DropTable("dbo.Chats");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Applications");
        }
    }
}
