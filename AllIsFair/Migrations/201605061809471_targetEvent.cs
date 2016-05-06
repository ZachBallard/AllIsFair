namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class targetEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Combatants", "Event_Id", c => c.Int());
            AddColumn("dbo.Events", "TargetNumber", c => c.Int(nullable: false));
            CreateIndex("dbo.Combatants", "Event_Id");
            AddForeignKey("dbo.Combatants", "Event_Id", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Combatants", "Event_Id", "dbo.Events");
            DropIndex("dbo.Combatants", new[] { "Event_Id" });
            DropColumn("dbo.Events", "TargetNumber");
            DropColumn("dbo.Combatants", "Event_Id");
        }
    }
}
