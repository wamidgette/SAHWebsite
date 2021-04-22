namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class danyal_new2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "StartOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.Courses", "OrderOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "OrderOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.Courses", "StartOn");
        }
    }
}
