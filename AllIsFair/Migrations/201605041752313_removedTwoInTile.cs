namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedTwoInTile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "WeaponRange", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "WeaponRange");
        }
    }
}
