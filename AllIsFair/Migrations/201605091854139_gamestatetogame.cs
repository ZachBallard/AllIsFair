namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gamestatetogame : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Tiles", "Game_Id", "dbo.Games");
            AddColumn("dbo.Games", "PlayerDone", c => c.Boolean(nullable: false));
            AddColumn("dbo.Games", "AskWeapon", c => c.Boolean(nullable: false));
            AddColumn("dbo.Games", "AskItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.Games", "ShowResult", c => c.Boolean(nullable: false));
            AddColumn("dbo.Games", "IsAttack", c => c.Boolean(nullable: false));
            AddColumn("dbo.Games", "Event_Id", c => c.Int());
            AddColumn("dbo.Events", "Game_Id1", c => c.Int());
            AddColumn("dbo.Tiles", "Game_Id1", c => c.Int());
            AddColumn("dbo.Tiles", "Game_Id2", c => c.Int());
            CreateIndex("dbo.Games", "Event_Id");
            CreateIndex("dbo.Events", "Game_Id1");
            CreateIndex("dbo.Tiles", "Game_Id1");
            CreateIndex("dbo.Tiles", "Game_Id2");
            AddForeignKey("dbo.Games", "Event_Id", "dbo.Events", "Id");
            AddForeignKey("dbo.Tiles", "Game_Id1", "dbo.Games", "Id");
            AddForeignKey("dbo.Events", "Game_Id1", "dbo.Games", "Id");
            AddForeignKey("dbo.Tiles", "Game_Id2", "dbo.Games", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tiles", "Game_Id2", "dbo.Games");
            DropForeignKey("dbo.Events", "Game_Id1", "dbo.Games");
            DropForeignKey("dbo.Tiles", "Game_Id1", "dbo.Games");
            DropForeignKey("dbo.Games", "Event_Id", "dbo.Events");
            DropIndex("dbo.Tiles", new[] { "Game_Id2" });
            DropIndex("dbo.Tiles", new[] { "Game_Id1" });
            DropIndex("dbo.Events", new[] { "Game_Id1" });
            DropIndex("dbo.Games", new[] { "Event_Id" });
            DropColumn("dbo.Tiles", "Game_Id2");
            DropColumn("dbo.Tiles", "Game_Id1");
            DropColumn("dbo.Events", "Game_Id1");
            DropColumn("dbo.Games", "Event_Id");
            DropColumn("dbo.Games", "IsAttack");
            DropColumn("dbo.Games", "ShowResult");
            DropColumn("dbo.Games", "AskItem");
            DropColumn("dbo.Games", "AskWeapon");
            DropColumn("dbo.Games", "PlayerDone");
            AddForeignKey("dbo.Tiles", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Events", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
        }
    }
}
