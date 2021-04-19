namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mahsa_update_appointment_table : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "DateOfBirth", c => c.DateTime());
            AlterColumn("dbo.Appointments", "AppintmentDateTime", c => c.DateTime());
            AlterColumn("dbo.Appointments", "IsFirstTimeVisit", c => c.Boolean());
            AlterColumn("dbo.Appointments", "IsUrgent", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "IsUrgent", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Appointments", "IsFirstTimeVisit", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Appointments", "AppintmentDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Appointments", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
