namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mahsa_update_faq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faqs", "Publish", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faqs", "Publish");
        }
    }
}
