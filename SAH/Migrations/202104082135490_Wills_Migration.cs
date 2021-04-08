namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Wills_Migration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "Chat_ChatId", "dbo.Chats");
            DropIndex("dbo.Messages", new[] { "Chat_ChatId" });
            RenameColumn(table: "dbo.Messages", name: "Chat_ChatId", newName: "ChatId");
            AlterColumn("dbo.Messages", "ChatId", c => c.Int(nullable: false));
            CreateIndex("dbo.Messages", "ChatId");
            AddForeignKey("dbo.Messages", "ChatId", "dbo.Chats", "ChatId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ChatId", "dbo.Chats");
            DropIndex("dbo.Messages", new[] { "ChatId" });
            AlterColumn("dbo.Messages", "ChatId", c => c.Int());
            RenameColumn(table: "dbo.Messages", name: "ChatId", newName: "Chat_ChatId");
            CreateIndex("dbo.Messages", "Chat_ChatId");
            AddForeignKey("dbo.Messages", "Chat_ChatId", "dbo.Chats", "ChatId");
        }
    }
}
