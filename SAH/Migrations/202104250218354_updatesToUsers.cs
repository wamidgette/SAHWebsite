namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatesToUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChatUsers", "Chat_ChatId", "dbo.Chats");
            DropForeignKey("dbo.ChatUsers", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Chats", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Users");
            DropIndex("dbo.Chats", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Messages", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ChatUsers", new[] { "Chat_ChatId" });
            DropIndex("dbo.ChatUsers", new[] { "User_UserId" });
            DropColumn("dbo.Messages", "SenderId");
            RenameColumn(table: "dbo.Messages", name: "ApplicationUser_Id", newName: "SenderId");
            CreateTable(
                "dbo.ApplicationUserChats",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Chat_ChatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Chat_ChatId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Chats", t => t.Chat_ChatId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Chat_ChatId);
            
            AddColumn("dbo.Chats", "User_UserId", c => c.Int());
            AddColumn("dbo.Messages", "User_UserId", c => c.Int());
            AlterColumn("dbo.Messages", "SenderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Chats", "User_UserId");
            CreateIndex("dbo.Messages", "SenderId");
            CreateIndex("dbo.Messages", "User_UserId");
            AddForeignKey("dbo.Chats", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Messages", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Chats", "ApplicationUser_Id");
            DropTable("dbo.ChatUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ChatUsers",
                c => new
                    {
                        Chat_ChatId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Chat_ChatId, t.User_UserId });
            
            AddColumn("dbo.Chats", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Chats", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.ApplicationUserChats", "Chat_ChatId", "dbo.Chats");
            DropForeignKey("dbo.ApplicationUserChats", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserChats", new[] { "Chat_ChatId" });
            DropIndex("dbo.ApplicationUserChats", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Messages", new[] { "User_UserId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Chats", new[] { "User_UserId" });
            AlterColumn("dbo.Messages", "SenderId", c => c.Int(nullable: false));
            DropColumn("dbo.Messages", "User_UserId");
            DropColumn("dbo.Chats", "User_UserId");
            DropTable("dbo.ApplicationUserChats");
            RenameColumn(table: "dbo.Messages", name: "SenderId", newName: "ApplicationUser_Id");
            AddColumn("dbo.Messages", "SenderId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChatUsers", "User_UserId");
            CreateIndex("dbo.ChatUsers", "Chat_ChatId");
            CreateIndex("dbo.Messages", "ApplicationUser_Id");
            CreateIndex("dbo.Messages", "SenderId");
            CreateIndex("dbo.Chats", "ApplicationUser_Id");
            AddForeignKey("dbo.Messages", "SenderId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Chats", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ChatUsers", "User_UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.ChatUsers", "Chat_ChatId", "dbo.Chats", "ChatId", cascadeDelete: true);
        }
    }
}
