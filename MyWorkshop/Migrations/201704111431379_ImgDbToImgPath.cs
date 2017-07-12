namespace MyWorkshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImgDbToImgPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "ImagePath", c => c.String());
            DropColumn("dbo.Posts", "ImageData");
            DropColumn("dbo.Posts", "ImageMimeType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "ImageMimeType", c => c.String());
            AddColumn("dbo.Posts", "ImageData", c => c.Binary());
            DropColumn("dbo.Posts", "ImagePath");
        }
    }
}
