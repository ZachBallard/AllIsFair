namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resultisattack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "IsAttack", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "IsAttack");
        }
    }
}
