namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BarbaraApplicationTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Applications", "ApplicationHasFile");
            DropColumn("dbo.Applications", "FileExtension");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Applications", "FileExtension", c => c.String());
            AddColumn("dbo.Applications", "ApplicationHasFile", c => c.String());
        }
    }
}
