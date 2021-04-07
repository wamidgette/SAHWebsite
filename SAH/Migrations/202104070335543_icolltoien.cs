namespace SAH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class icolltoien : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chats", "User_UserId", "dbo.Users");
            DropIndex("dbo.Chats", new[] { "User_UserId" });
            DropColumn("dbo.Chats", "User_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chats", "User_UserId", c => c.Int());
            CreateIndex("dbo.Chats", "User_UserId");
            AddForeignKey("dbo.Chats", "User_UserId", "dbo.Users", "UserId");
        }
    }
}
