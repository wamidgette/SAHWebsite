namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class danyalchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeApplicants", "Reason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeApplicants", "Reason");
        }
    }
}
