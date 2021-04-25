namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateapplicationmodel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserChats", newName: "ChatApplicationUsers");
            DropForeignKey("dbo.Applications", "UserId", "dbo.Users");
            DropIndex("dbo.Applications", new[] { "UserId" });
            RenameColumn(table: "dbo.Applications", name: "UserId", newName: "User_UserId");
            RenameColumn(table: "dbo.Applications", name: "ApplicationUser_Id", newName: "Id");
            RenameIndex(table: "dbo.Applications", name: "IX_ApplicationUser_Id", newName: "IX_Id");
            DropPrimaryKey("dbo.ChatApplicationUsers");
            AlterColumn("dbo.Applications", "User_UserId", c => c.Int());
            AddPrimaryKey("dbo.ChatApplicationUsers", new[] { "Chat_ChatId", "ApplicationUser_Id" });
            CreateIndex("dbo.Applications", "User_UserId");
            AddForeignKey("dbo.Applications", "User_UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Applications", "User_UserId", "dbo.Users");
            DropIndex("dbo.Applications", new[] { "User_UserId" });
            DropPrimaryKey("dbo.ChatApplicationUsers");
            AlterColumn("dbo.Applications", "User_UserId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ChatApplicationUsers", new[] { "ApplicationUser_Id", "Chat_ChatId" });
            RenameIndex(table: "dbo.Applications", name: "IX_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Applications", name: "Id", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Applications", name: "User_UserId", newName: "UserId");
            CreateIndex("dbo.Applications", "UserId");
            AddForeignKey("dbo.Applications", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            RenameTable(name: "dbo.ChatApplicationUsers", newName: "ApplicationUserChats");
        }
    }
}
