namespace AllIsFair.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rollstoint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rolls", "Result_Id", "dbo.Results");
            DropIndex("dbo.Rolls", new[] { "Result_Id" });
            DropTable("dbo.Rolls");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Rolls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        Result_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Rolls", "Result_Id");
            AddForeignKey("dbo.Rolls", "Result_Id", "dbo.Results", "Id");
        }
    }
}
