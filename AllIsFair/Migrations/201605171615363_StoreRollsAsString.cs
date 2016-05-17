namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreRollsAsString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "Rolls", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "Rolls");
        }
    }
}
