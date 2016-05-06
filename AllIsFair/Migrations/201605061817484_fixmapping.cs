namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixmapping : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Combatants", "Event_Id", "dbo.Events");
            DropIndex("dbo.Combatants", new[] { "Event_Id" });
            DropColumn("dbo.Combatants", "DieResults");
            DropColumn("dbo.Combatants", "Event_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Combatants", "Event_Id", c => c.Int());
            AddColumn("dbo.Combatants", "DieResults", c => c.Int(nullable: false));
            CreateIndex("dbo.Combatants", "Event_Id");
            AddForeignKey("dbo.Combatants", "Event_Id", "dbo.Events", "Id");
        }
    }
}
