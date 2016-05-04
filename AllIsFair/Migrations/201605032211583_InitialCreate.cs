namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .ForeignKey("dbo.Tiles", t => t.Tile_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.Combatant_Id)
                .Index(t => t.Tile_Id);
            
            CreateTable(
                "dbo.Effects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Combatant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .Index(t => t.Combatant_Id);
            
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
                        IsPlayer = c.Boolean(nullable: false),
                        Strength = c.Int(nullable: false),
                        Speed = c.Int(nullable: false),
                        Sanity = c.Int(nullable: false),
                        GraphicName = c.String(),
                        Game_Id = c.Int(nullable: false),
                        Killer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .ForeignKey("dbo.Combatants", t => t.Killer_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.Killer_Id);
            
            CreateTable(
                "dbo.Tiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GraphicName = c.String(),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Combatant_Id = c.Int(),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.Combatant_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.EffectCards",
                c => new
                    {
                        Effect_Id = c.Int(nullable: false),
                        Card_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Effect_Id, t.Card_Id })
                .ForeignKey("dbo.Effects", t => t.Effect_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cards", t => t.Card_Id, cascadeDelete: true)
                .Index(t => t.Effect_Id)
                .Index(t => t.Card_Id);
            
            CreateTable(
                "dbo.EffectGames",
                c => new
                    {
                        Effect_Id = c.Int(nullable: false),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Effect_Id, t.Game_Id })
                .ForeignKey("dbo.Effects", t => t.Effect_Id, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.Effect_Id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Games", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tiles", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Tiles", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Cards", "Tile_Id", "dbo.Tiles");
            DropForeignKey("dbo.Combatants", "Killer_Id", "dbo.Combatants");
            DropForeignKey("dbo.Combatants", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Effects", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Cards", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Cards", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.EffectGames", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.EffectGames", "Effect_Id", "dbo.Effects");
            DropForeignKey("dbo.EffectCards", "Card_Id", "dbo.Cards");
            DropForeignKey("dbo.EffectCards", "Effect_Id", "dbo.Effects");
            DropIndex("dbo.EffectGames", new[] { "Game_Id" });
            DropIndex("dbo.EffectGames", new[] { "Effect_Id" });
            DropIndex("dbo.EffectCards", new[] { "Card_Id" });
            DropIndex("dbo.EffectCards", new[] { "Effect_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Tiles", new[] { "Game_Id" });
            DropIndex("dbo.Tiles", new[] { "Combatant_Id" });
            DropIndex("dbo.Combatants", new[] { "Killer_Id" });
            DropIndex("dbo.Combatants", new[] { "Game_Id" });
            DropIndex("dbo.Effects", new[] { "Combatant_Id" });
            DropIndex("dbo.Cards", new[] { "Tile_Id" });
            DropIndex("dbo.Cards", new[] { "Combatant_Id" });
            DropIndex("dbo.Cards", new[] { "Game_Id" });
            DropIndex("dbo.Games", new[] { "User_Id" });
            DropTable("dbo.EffectGames");
            DropTable("dbo.EffectCards");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Tiles");
            DropTable("dbo.Combatants");
            DropTable("dbo.Effects");
            DropTable("dbo.Cards");
            DropTable("dbo.Games");
        }
    }
}
