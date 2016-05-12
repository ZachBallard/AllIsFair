namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixedtables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Game_Id", "dbo.Games");
            DropIndex("dbo.Items", new[] { "Game_Id" });
            AddColumn("dbo.Games", "CurrentTurnNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "CurrentTurnOrder", c => c.Int(nullable: false));
            AddColumn("dbo.Combatants", "TurnOrder", c => c.Int(nullable: false));
            DropColumn("dbo.Games", "CombatantTurn");
            DropColumn("dbo.Combatants", "TurnNumber");
            DropColumn("dbo.Items", "Game_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "Game_Id", c => c.Int());
            AddColumn("dbo.Combatants", "TurnNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "CombatantTurn", c => c.Int(nullable: false));
            DropColumn("dbo.Combatants", "TurnOrder");
            DropColumn("dbo.Games", "CurrentTurnOrder");
            DropColumn("dbo.Games", "CurrentTurnNumber");
            CreateIndex("dbo.Items", "Game_Id");
            AddForeignKey("dbo.Items", "Game_Id", "dbo.Games", "Id");
        }
    }
}
