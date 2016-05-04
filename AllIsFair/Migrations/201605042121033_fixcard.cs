namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixcard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "CombatantTurn", c => c.Int(nullable: false));
            AddColumn("dbo.Combatants", "TurnNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Combatants", "TurnNumber");
            DropColumn("dbo.Games", "CombatantTurn");
        }
    }
}
