namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class danyal_new : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "OrderOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "OrderOn");
        }
    }
}
