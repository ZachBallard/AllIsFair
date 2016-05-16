namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedGameReq : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Results", "Game_Id", "dbo.Games");
            DropIndex("dbo.Results", new[] { "Game_Id" });
            DropIndex("dbo.Events", new[] { "Game_Id" });
            DropIndex("dbo.Events", new[] { "Game_Id1" });
            DropColumn("dbo.Events", "Game_Id");
            RenameColumn(table: "dbo.Events", name: "Game_Id1", newName: "Game_Id");
            AlterColumn("dbo.Events", "Game_Id", c => c.Int());
            CreateIndex("dbo.Events", "Game_Id");
            DropColumn("dbo.Results", "Game_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Results", "Game_Id", c => c.Int(nullable: false));
            DropIndex("dbo.Events", new[] { "Game_Id" });
            AlterColumn("dbo.Events", "Game_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Events", name: "Game_Id", newName: "Game_Id1");
            AddColumn("dbo.Events", "Game_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Events", "Game_Id1");
            CreateIndex("dbo.Events", "Game_Id");
            CreateIndex("dbo.Results", "Game_Id");
            AddForeignKey("dbo.Results", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Events", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
        }
    }
}
