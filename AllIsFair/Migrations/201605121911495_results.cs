namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class results : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TurnNumber = c.Int(nullable: false),
                        Healthloss = c.Int(nullable: false),
                        Combatant_Id = c.Int(),
                        Event_Id = c.Int(),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.Combatant_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Rolls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        Result_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Results", t => t.Result_Id)
                .Index(t => t.Result_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rolls", "Result_Id", "dbo.Results");
            DropForeignKey("dbo.Results", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Results", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Results", "Combatant_Id", "dbo.Combatants");
            DropIndex("dbo.Rolls", new[] { "Result_Id" });
            DropIndex("dbo.Results", new[] { "Game_Id" });
            DropIndex("dbo.Results", new[] { "Event_Id" });
            DropIndex("dbo.Results", new[] { "Combatant_Id" });
            DropTable("dbo.Rolls");
            DropTable("dbo.Results");
        }
    }
}
