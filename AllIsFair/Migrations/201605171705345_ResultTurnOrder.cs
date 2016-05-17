namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResultTurnOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "TurnOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "TurnOrder");
        }
    }
}
