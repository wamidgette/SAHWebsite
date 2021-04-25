namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ikumiupdatedonationmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Donations", "UserId", "dbo.Users");
            DropIndex("dbo.Donations", new[] { "UserId" });
            RenameColumn(table: "dbo.Donations", name: "ApplicationUser_Id", newName: "Id");
            RenameColumn(table: "dbo.Donations", name: "UserId", newName: "User_UserId");
            RenameIndex(table: "dbo.Donations", name: "IX_ApplicationUser_Id", newName: "IX_Id");
            AlterColumn("dbo.Donations", "User_UserId", c => c.Int());
            CreateIndex("dbo.Donations", "User_UserId");
            AddForeignKey("dbo.Donations", "User_UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "User_UserId", "dbo.Users");
            DropIndex("dbo.Donations", new[] { "User_UserId" });
            AlterColumn("dbo.Donations", "User_UserId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Donations", name: "IX_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Donations", name: "User_UserId", newName: "UserId");
            RenameColumn(table: "dbo.Donations", name: "Id", newName: "ApplicationUser_Id");
            CreateIndex("dbo.Donations", "UserId");
            AddForeignKey("dbo.Donations", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
