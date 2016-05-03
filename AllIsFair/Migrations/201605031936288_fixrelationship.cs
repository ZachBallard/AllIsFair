namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixrelationship : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Games", new[] { "ApplicationUser_Id" });
            RenameColumn(table: "dbo.Games", name: "ApplicationUser_Id", newName: "User_Id");
            AlterColumn("dbo.Games", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Games", "User_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Games", new[] { "User_Id" });
            AlterColumn("dbo.Games", "User_Id", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.Games", name: "User_Id", newName: "ApplicationUser_Id");
            CreateIndex("dbo.Games", "ApplicationUser_Id");
        }
    }
}
