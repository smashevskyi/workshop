namespace MyWorkshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removed_albumViewModel_fromDB : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AlbumViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AlbumViewModels",
                c => new
                    {
                        AlbumId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Slug = c.String(nullable: false, maxLength: 30),
                        Description = c.String(nullable: false, maxLength: 250),
                        Occasion = c.String(maxLength: 30),
                        Published = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AlbumId);
            
        }
    }
}
