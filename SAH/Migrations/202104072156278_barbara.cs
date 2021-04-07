namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class barbara : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        ApplicationHasFile = c.String(),
                        FileExtension = c.String(),
                        UserId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicationId)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.JobId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        Position = c.String(),
                        Category = c.String(),
                        Type = c.String(),
                        Requirement = c.String(),
                        Deadline = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobId);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        ChatId = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ChatId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        DateSent = c.DateTime(nullable: false),
                        Content = c.String(),
                        Chat_ChatId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Users", t => t.SenderId, cascadeDelete: true)
                .ForeignKey("dbo.Chats", t => t.Chat_ChatId)
                .Index(t => t.SenderId)
                .Index(t => t.Chat_ChatId);
            
            CreateTable(
                "dbo.ChatUsers",
                c => new
                    {
                        Chat_ChatId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Chat_ChatId, t.User_UserId })
                .ForeignKey("dbo.Chats", t => t.Chat_ChatId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Chat_ChatId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Applications", "UserId", "dbo.Users");
            DropForeignKey("dbo.ChatUsers", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.ChatUsers", "Chat_ChatId", "dbo.Chats");
            DropForeignKey("dbo.Messages", "Chat_ChatId", "dbo.Chats");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.Applications", "JobId", "dbo.Jobs");
            DropIndex("dbo.ChatUsers", new[] { "User_UserId" });
            DropIndex("dbo.ChatUsers", new[] { "Chat_ChatId" });
            DropIndex("dbo.Messages", new[] { "Chat_ChatId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Applications", new[] { "JobId" });
            DropIndex("dbo.Applications", new[] { "UserId" });
            DropTable("dbo.ChatUsers");
            DropTable("dbo.Messages");
            DropTable("dbo.Chats");
            DropTable("dbo.Jobs");
            DropTable("dbo.Applications");
        }
    }
}
