namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class danyal_improv : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmployeeApplicants", "Reason", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeApplicants", "Reason", c => c.Int(nullable: false));
        }
    }
}
