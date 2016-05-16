namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noreqcombatantonitem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Combatant_Id", "dbo.Combatants");
            DropIndex("dbo.Items", new[] { "Combatant_Id" });
            AlterColumn("dbo.Items", "Combatant_Id", c => c.Int());
            CreateIndex("dbo.Items", "Combatant_Id");
            AddForeignKey("dbo.Items", "Combatant_Id", "dbo.Combatants", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Combatant_Id", "dbo.Combatants");
            DropIndex("dbo.Items", new[] { "Combatant_Id" });
            AlterColumn("dbo.Items", "Combatant_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Items", "Combatant_Id");
            AddForeignKey("dbo.Items", "Combatant_Id", "dbo.Combatants", "Id", cascadeDelete: true);
        }
    }
}
