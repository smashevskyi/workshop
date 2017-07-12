namespace MyWorkshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class another_bug_with_EF_migrations_trying_to_fix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumViewModels",
                c => new
                    {
                        AlbumId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Slug = c.String(nullable: false, maxLength: 30),
                        Description = c.String(nullable: false, maxLength: 250),
                        PreviewImagePath = c.String(),
                        Occasion = c.String(maxLength: 30),
                        Published = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AlbumId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AlbumViewModels");
        }
    }
}
