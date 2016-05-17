namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eventOnItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "ItemReward_Id", "dbo.Items");
            DropIndex("dbo.Events", new[] { "ItemReward_Id" });
            DropColumn("dbo.Items", "Id");
            RenameColumn(table: "dbo.Items", name: "ItemReward_Id", newName: "Id");
            DropPrimaryKey("dbo.Items");
            AlterColumn("dbo.Items", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Items", "Id");
            CreateIndex("dbo.Items", "Id");
            DropColumn("dbo.Events", "ItemReward_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "ItemReward_Id", c => c.Int());
            DropIndex("dbo.Items", new[] { "Id" });
            DropPrimaryKey("dbo.Items");
            AlterColumn("dbo.Items", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Items", "Id");
            RenameColumn(table: "dbo.Items", name: "Id", newName: "ItemReward_Id");
            AddColumn("dbo.Items", "Id", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Events", "ItemReward_Id");
            AddForeignKey("dbo.Events", "ItemReward_Id", "dbo.Items", "Id");
        }
    }
}
