namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class game : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Combatants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Points = c.Int(nullable: false),
                        Energy = c.Int(nullable: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Strength = c.Int(nullable: false),
                        Speed = c.Int(nullable: false),
                        Sanity = c.Int(nullable: false),
                        Killer_Id = c.Int(),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Combatants", t => t.Killer_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Killer_Id)
                .Index(t => t.Game_Id);
            
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
                        Combatant_Id = c.Int(),
                        Game_Id = c.Int(),
                        Game_Id1 = c.Int(),
                        Game_Id2 = c.Int(),
                        Tile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id1)
                .ForeignKey("dbo.Games", t => t.Game_Id2)
                .ForeignKey("dbo.Tiles", t => t.Tile_Id)
                .Index(t => t.Combatant_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.Game_Id1)
                .Index(t => t.Game_Id2)
                .Index(t => t.Tile_Id);
            
            CreateTable(
                "dbo.Effects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Card_Id = c.Int(),
                        Combatant_Id = c.Int(),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Card_Id)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Card_Id)
                .Index(t => t.Combatant_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Tiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GraphicName = c.String(),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tiles", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Cards", "Tile_Id", "dbo.Tiles");
            DropForeignKey("dbo.Cards", "Game_Id2", "dbo.Games");
            DropForeignKey("dbo.Cards", "Game_Id1", "dbo.Games");
            DropForeignKey("dbo.Cards", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Effects", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Combatants", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Combatants", "Killer_Id", "dbo.Combatants");
            DropForeignKey("dbo.Effects", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Cards", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Effects", "Card_Id", "dbo.Cards");
            DropIndex("dbo.Tiles", new[] { "Game_Id" });
            DropIndex("dbo.Effects", new[] { "Game_Id" });
            DropIndex("dbo.Effects", new[] { "Combatant_Id" });
            DropIndex("dbo.Effects", new[] { "Card_Id" });
            DropIndex("dbo.Cards", new[] { "Tile_Id" });
            DropIndex("dbo.Cards", new[] { "Game_Id2" });
            DropIndex("dbo.Cards", new[] { "Game_Id1" });
            DropIndex("dbo.Cards", new[] { "Game_Id" });
            DropIndex("dbo.Cards", new[] { "Combatant_Id" });
            DropIndex("dbo.Combatants", new[] { "Game_Id" });
            DropIndex("dbo.Combatants", new[] { "Killer_Id" });
            DropIndex("dbo.Games", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Tiles");
            DropTable("dbo.Effects");
            DropTable("dbo.Cards");
            DropTable("dbo.Combatants");
            DropTable("dbo.Games");
        }
    }
}
