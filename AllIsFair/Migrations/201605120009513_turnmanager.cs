namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class turnmanager : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Combatants", "TurnManager_Id", "dbo.TurnManagers");
            DropForeignKey("dbo.Games", "TurnManager_Id", "dbo.TurnManagers");
            DropIndex("dbo.Games", new[] { "TurnManager_Id" });
            DropColumn("dbo.TurnManagers", "Id");
            RenameColumn(table: "dbo.TurnManagers", name: "TurnManager_Id", newName: "Id");
            DropPrimaryKey("dbo.TurnManagers");
            AlterColumn("dbo.TurnManagers", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TurnManagers", "Id");
            CreateIndex("dbo.TurnManagers", "Id");
            AddForeignKey("dbo.Combatants", "TurnManager_Id", "dbo.TurnManagers", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "TurnManager_Id", c => c.Int());
            DropForeignKey("dbo.Combatants", "TurnManager_Id", "dbo.TurnManagers");
            DropIndex("dbo.TurnManagers", new[] { "Id" });
            DropPrimaryKey("dbo.TurnManagers");
            AlterColumn("dbo.TurnManagers", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.TurnManagers", "Id");
            RenameColumn(table: "dbo.TurnManagers", name: "Id", newName: "TurnManager_Id");
            AddColumn("dbo.TurnManagers", "Id", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Games", "TurnManager_Id");
            AddForeignKey("dbo.Games", "TurnManager_Id", "dbo.TurnManagers", "Id");
            AddForeignKey("dbo.Combatants", "TurnManager_Id", "dbo.TurnManagers", "Id");
        }
    }
}
