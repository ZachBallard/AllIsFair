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
                        CurrentTurnNumber = c.Int(nullable: false),
                        CurrentTurnOrder = c.Int(nullable: false),
                        Event_Id = c.Int(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Combatants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TurnOrder = c.Int(nullable: false),
                        Name = c.String(),
                        IsPlayer = c.Boolean(nullable: false),
                        Strength = c.Int(nullable: false),
                        Speed = c.Int(nullable: false),
                        Sanity = c.Int(nullable: false),
                        Perception = c.Int(nullable: false),
                        Health = c.Int(nullable: false),
                        GraphicName = c.String(),
                        Game_Id = c.Int(nullable: false),
                        Killer_Id = c.Int(),
                        Tile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .ForeignKey("dbo.Combatants", t => t.Killer_Id)
                .ForeignKey("dbo.Tiles", t => t.Tile_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.Killer_Id)
                .Index(t => t.Tile_Id);
            
            CreateTable(
                "dbo.GameActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Action = c.Int(nullable: false),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                        Combatant_Id = c.Int(),
                        CurrentGame_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .ForeignKey("dbo.Games", t => t.CurrentGame_Id, cascadeDelete: true)
                .Index(t => t.Combatant_Id)
                .Index(t => t.CurrentGame_Id);
            
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
                        WeaponRange = c.Int(nullable: false),
                        SurvivalBonus = c.Int(nullable: false),
                        ThreatBonus = c.Int(nullable: false),
                        Combatant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .Index(t => t.Combatant_Id);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TurnNumber = c.Int(nullable: false),
                        TurnOrder = c.Int(nullable: false),
                        Healthloss = c.Int(nullable: false),
                        StatReward = c.Int(nullable: false),
                        IsAttack = c.Boolean(nullable: false),
                        Rolls = c.String(),
                        Combatant_Id = c.Int(),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Combatants", t => t.Combatant_Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Combatant_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GraphicName = c.String(),
                        RequiredStat = c.Int(nullable: false),
                        TargetNumber = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        StatReward = c.Int(nullable: false),
                        Description = c.String(),
                        ItemReward_Id = c.Int(),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemReward_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.ItemReward_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Tiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GraphicName = c.String(),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Games", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Games", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Combatants", "Tile_Id", "dbo.Tiles");
            DropForeignKey("dbo.Tiles", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Results", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Events", "ItemReward_Id", "dbo.Items");
            DropForeignKey("dbo.Results", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Combatants", "Killer_Id", "dbo.Combatants");
            DropForeignKey("dbo.Items", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.GameActions", "CurrentGame_Id", "dbo.Games");
            DropForeignKey("dbo.GameActions", "Combatant_Id", "dbo.Combatants");
            DropForeignKey("dbo.Combatants", "Game_Id", "dbo.Games");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Tiles", new[] { "Game_Id" });
            DropIndex("dbo.Events", new[] { "Game_Id" });
            DropIndex("dbo.Events", new[] { "ItemReward_Id" });
            DropIndex("dbo.Results", new[] { "Event_Id" });
            DropIndex("dbo.Results", new[] { "Combatant_Id" });
            DropIndex("dbo.Items", new[] { "Combatant_Id" });
            DropIndex("dbo.GameActions", new[] { "CurrentGame_Id" });
            DropIndex("dbo.GameActions", new[] { "Combatant_Id" });
            DropIndex("dbo.Combatants", new[] { "Tile_Id" });
            DropIndex("dbo.Combatants", new[] { "Killer_Id" });
            DropIndex("dbo.Combatants", new[] { "Game_Id" });
            DropIndex("dbo.Games", new[] { "User_Id" });
            DropIndex("dbo.Games", new[] { "Event_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Tiles");
            DropTable("dbo.Events");
            DropTable("dbo.Results");
            DropTable("dbo.Items");
            DropTable("dbo.GameActions");
            DropTable("dbo.Combatants");
            DropTable("dbo.Games");
        }
    }
}
