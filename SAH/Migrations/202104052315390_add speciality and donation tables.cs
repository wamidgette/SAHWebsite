namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addspecialityanddonationtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        DonationId = c.Int(nullable: false, identity: true),
                        AmountOfDonation = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMethod = c.String(),
                        DonationDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonationId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Specialities",
                c => new
                    {
                        SpecialityId = c.Int(nullable: false, identity: true),
                        SpecialityName = c.String(),
                    })
                .PrimaryKey(t => t.SpecialityId);
            
            CreateIndex("dbo.Users", "SpecialityId");
            CreateIndex("dbo.Users", "DepartmentId");
            AddForeignKey("dbo.Users", "DepartmentId", "dbo.Departments", "DepartmentId");
            AddForeignKey("dbo.Users", "SpecialityId", "dbo.Specialities", "SpecialityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "SpecialityId", "dbo.Specialities");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.Users", new[] { "SpecialityId" });
            DropIndex("dbo.Donations", new[] { "UserId" });
            DropTable("dbo.Specialities");
            DropTable("dbo.Donations");
        }
    }
}
