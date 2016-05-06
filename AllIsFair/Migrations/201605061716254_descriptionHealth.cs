namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class descriptionHealth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Combatants", "Health", c => c.Int(nullable: false));
            AddColumn("dbo.Events", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Description");
            DropColumn("dbo.Combatants", "Health");
        }
    }
}
