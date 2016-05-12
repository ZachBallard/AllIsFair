namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class turnmanagergame : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TurnManagers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsPlayerAction = c.Boolean(nullable: false),
                        Healthloss = c.Int(nullable: false),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Event_Id);
            
            AddColumn("dbo.Games", "TurnManager_Id", c => c.Int());
            AddColumn("dbo.Combatants", "TurnManager_Id", c => c.Int());
            CreateIndex("dbo.Games", "TurnManager_Id");
            CreateIndex("dbo.Combatants", "TurnManager_Id");
            AddForeignKey("dbo.Combatants", "TurnManager_Id", "dbo.TurnManagers", "Id");
            AddForeignKey("dbo.Games", "TurnManager_Id", "dbo.TurnManagers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "TurnManager_Id", "dbo.TurnManagers");
            DropForeignKey("dbo.Combatants", "TurnManager_Id", "dbo.TurnManagers");
            DropForeignKey("dbo.TurnManagers", "Event_Id", "dbo.Events");
            DropIndex("dbo.TurnManagers", new[] { "Event_Id" });
            DropIndex("dbo.Combatants", new[] { "TurnManager_Id" });
            DropIndex("dbo.Games", new[] { "TurnManager_Id" });
            DropColumn("dbo.Combatants", "TurnManager_Id");
            DropColumn("dbo.Games", "TurnManager_Id");
            DropTable("dbo.TurnManagers");
        }
    }
}
