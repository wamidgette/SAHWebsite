namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class danyal_tables2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseCode = c.String(),
                        CourseName = c.String(),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.EmployeeApplicants",
                c => new
                    {
                        EmployeeApplicantId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeApplicantId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeApplicants", "UserId", "dbo.Users");
            DropForeignKey("dbo.EmployeeApplicants", "CourseId", "dbo.Courses");
            DropIndex("dbo.EmployeeApplicants", new[] { "CourseId" });
            DropIndex("dbo.EmployeeApplicants", new[] { "UserId" });
            DropTable("dbo.EmployeeApplicants");
            DropTable("dbo.Courses");
        }
    }
}
