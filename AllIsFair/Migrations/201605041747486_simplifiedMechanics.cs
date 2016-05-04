namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class simplifiedMechanics : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EffectCards", "Effect_Id", "dbo.Effects");
            DropForeignKey("dbo.EffectCards", "Card_Id", "dbo.Cards");
            DropForeignKey("dbo.EffectGames", "Effect_Id", "dbo.Effects");
            DropForeignKey("dbo.EffectGames", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Cards", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Cards", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Effects", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Cards", "Tile_Id", "dbo.Tiles");
            DropIndex("dbo.Cards", new[] { "Game_Id" });
            DropIndex("dbo.Cards", new[] { "Combatant_Id" });
            DropIndex("dbo.Cards", new[] { "Tile_Id" });
            DropIndex("dbo.Effects", new[] { "Combatant_Id" });
            DropIndex("dbo.EffectCards", new[] { "Effect_Id" });
            DropIndex("dbo.EffectCards", new[] { "Card_Id" });
            DropIndex("dbo.EffectGames", new[] { "Effect_Id" });
            DropIndex("dbo.EffectGames", new[] { "Game_Id" });
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GraphicName = c.String(),
                        Counter = c.Int(nullable: false),
                        DoesCount = c.Boolean(nullable: false),
                        IsWeapon = c.Boolean(nullable: false),
                        SurvivalBonus = c.Int(nullable: false),
                        ThreatBonus = c.Int(nullable: false),
                        Game_Id = c.Int(nullable: false),
                        Combatant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.Combatant_Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GraphicName = c.String(),
                        RequiredStat = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        StatReward = c.Int(nullable: false),
                        Game_Id = c.Int(nullable: false),
                        ItemReward_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemReward_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.ItemReward_Id);
            
            AddColumn("dbo.Combatants", "Perception", c => c.Int(nullable: false));
            DropColumn("dbo.Combatants", "Points");
            DropColumn("dbo.Combatants", "Energy");
            DropTable("dbo.Cards");
            DropTable("dbo.Effects");
            DropTable("dbo.EffectCards");
            DropTable("dbo.EffectGames");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EffectGames",
                c => new
                    {
                        Effect_Id = c.Int(nullable: false),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Effect_Id, t.Game_Id });
            
            CreateTable(
                "dbo.EffectCards",
                c => new
                    {
                        Effect_Id = c.Int(nullable: false),
                        Card_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Effect_Id, t.Card_Id });
            
            CreateTable(
                "dbo.Effects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Combatant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GraphicName = c.String(),
                        Counter = c.Int(nullable: false),
                        DoesCount = c.Boolean(nullable: false),
                        IsOnce = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                        Game_Id = c.Int(nullable: false),
                        Combatant_Id = c.Int(),
                        Tile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Combatants", "Energy", c => c.Int(nullable: false));
            AddColumn("dbo.Combatants", "Points", c => c.Int(nullable: false));
            DropForeignKey("dbo.Events", "ItemReward_Id", "dbo.Items");
            DropForeignKey("dbo.Events", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Items", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Items", "Game_Id", "dbo.Games");
            DropIndex("dbo.Events", new[] { "ItemReward_Id" });
            DropIndex("dbo.Events", new[] { "Game_Id" });
            DropIndex("dbo.Items", new[] { "Combatant_Id" });
            DropIndex("dbo.Items", new[] { "Game_Id" });
            DropColumn("dbo.Combatants", "Perception");
            DropTable("dbo.Events");
            DropTable("dbo.Items");
            CreateIndex("dbo.EffectGames", "Game_Id");
            CreateIndex("dbo.EffectGames", "Effect_Id");
            CreateIndex("dbo.EffectCards", "Card_Id");
            CreateIndex("dbo.EffectCards", "Effect_Id");
            CreateIndex("dbo.Effects", "Combatant_Id");
            CreateIndex("dbo.Cards", "Tile_Id");
            CreateIndex("dbo.Cards", "Combatant_Id");
            CreateIndex("dbo.Cards", "Game_Id");
            AddForeignKey("dbo.Cards", "Tile_Id", "dbo.Tiles", "Id");
            AddForeignKey("dbo.Effects", "Combatant_Id", "dbo.Combatants", "Id");
            AddForeignKey("dbo.Cards", "Combatant_Id", "dbo.Combatants", "Id");
            AddForeignKey("dbo.Cards", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EffectGames", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EffectGames", "Effect_Id", "dbo.Effects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EffectCards", "Card_Id", "dbo.Cards", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EffectCards", "Effect_Id", "dbo.Effects", "Id", cascadeDelete: true);
        }
    }
}
