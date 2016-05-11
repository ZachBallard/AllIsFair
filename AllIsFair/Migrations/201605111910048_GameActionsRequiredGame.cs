namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameActionsRequiredGame : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameActions", "CurrentGame_Id", "dbo.Games");
            DropIndex("dbo.GameActions", new[] { "CurrentGame_Id" });
            AlterColumn("dbo.GameActions", "CurrentGame_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.GameActions", "CurrentGame_Id");
            AddForeignKey("dbo.GameActions", "CurrentGame_Id", "dbo.Games", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameActions", "CurrentGame_Id", "dbo.Games");
            DropIndex("dbo.GameActions", new[] { "CurrentGame_Id" });
            AlterColumn("dbo.GameActions", "CurrentGame_Id", c => c.Int());
            CreateIndex("dbo.GameActions", "CurrentGame_Id");
            AddForeignKey("dbo.GameActions", "CurrentGame_Id", "dbo.Games", "Id");
        }
    }
}
