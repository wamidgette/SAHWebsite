namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ikumi_migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "User_UserId", c => c.Int());
            AddColumn("dbo.Donations", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "User_UserId");
            CreateIndex("dbo.Donations", "DepartmentId");
            AddForeignKey("dbo.Courses", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Donations", "DepartmentId", "dbo.Departments", "DepartmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Courses", "User_UserId", "dbo.Users");
            DropIndex("dbo.Donations", new[] { "DepartmentId" });
            DropIndex("dbo.Courses", new[] { "User_UserId" });
            DropColumn("dbo.Donations", "DepartmentId");
            DropColumn("dbo.Courses", "User_UserId");
        }
    }
}
