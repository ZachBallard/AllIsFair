namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tileType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tiles", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tiles", "Type");
        }
    }
}
