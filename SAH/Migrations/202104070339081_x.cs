namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserChats",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Chat_ChatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Chat_ChatId })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Chats", t => t.Chat_ChatId, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Chat_ChatId);
            
            AddColumn("dbo.Messages", "Chat_ChatId", c => c.Int());
            CreateIndex("dbo.Messages", "Chat_ChatId");
            AddForeignKey("dbo.Messages", "Chat_ChatId", "dbo.Chats", "ChatId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Chat_ChatId", "dbo.Chats");
            DropForeignKey("dbo.UserChats", "Chat_ChatId", "dbo.Chats");
            DropForeignKey("dbo.UserChats", "User_UserId", "dbo.Users");
            DropIndex("dbo.UserChats", new[] { "Chat_ChatId" });
            DropIndex("dbo.UserChats", new[] { "User_UserId" });
            DropIndex("dbo.Messages", new[] { "Chat_ChatId" });
            DropColumn("dbo.Messages", "Chat_ChatId");
            DropTable("dbo.UserChats");
        }
    }
}
