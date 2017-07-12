namespace MyWorkshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Image_UrlSlug : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "UrlSlug", c => c.String());
            AddColumn("dbo.Posts", "ImageData", c => c.Binary());
            AddColumn("dbo.Posts", "ImageMimeType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "ImageMimeType");
            DropColumn("dbo.Posts", "ImageData");
            DropColumn("dbo.Posts", "UrlSlug");
        }
    }
}
