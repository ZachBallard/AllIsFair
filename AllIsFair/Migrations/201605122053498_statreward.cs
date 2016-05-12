namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class statreward : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "StatReward", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "StatReward");
        }
    }
}
