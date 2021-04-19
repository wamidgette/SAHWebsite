namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mahsa_update_appointment_table2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "IsFirstTimeVisit", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Appointments", "IsUrgent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "IsUrgent", c => c.Boolean());
            AlterColumn("dbo.Appointments", "IsFirstTimeVisit", c => c.Boolean());
        }
    }
}
