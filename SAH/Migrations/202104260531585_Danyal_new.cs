namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Danyal_new : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CourseDuration", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "CourseDuration");
        }
    }
}
