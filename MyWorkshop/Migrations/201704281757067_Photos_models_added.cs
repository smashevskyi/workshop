namespace MyWorkshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Photos_models_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Slug = c.String(),
                        Description = c.String(),
                        PreviewImagePath = c.String(),
                        Occasion = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifedOn = c.DateTime(nullable: false),
                        Published = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Position = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Published = c.Boolean(nullable: false),
                        ImagePath = c.String(),
                        ThumbPath = c.String(),
                        ResizedPath = c.String(),
                        Album_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.Album_Id)
                .Index(t => t.Album_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "Album_Id", "dbo.Albums");
            DropIndex("dbo.Images", new[] { "Album_Id" });
            DropTable("dbo.Images");
            DropTable("dbo.Albums");
        }
    }
}
