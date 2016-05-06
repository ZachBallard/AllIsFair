namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class playerDieResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Combatants", "DieResults", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Combatants", "DieResults");
        }
    }
}
