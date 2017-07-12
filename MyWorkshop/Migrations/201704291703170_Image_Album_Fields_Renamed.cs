namespace MyWorkshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Image_Album_Fields_Renamed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "Album_Id", "dbo.Albums");
            RenameColumn(table: "dbo.Images", name: "Album_Id", newName: "Album_AlbumId");
            RenameIndex(table: "dbo.Images", name: "IX_Album_Id", newName: "IX_Album_AlbumId");
            DropPrimaryKey("dbo.Albums");
            DropPrimaryKey("dbo.Images");
            DropColumn("dbo.Albums", "Id");
            DropColumn("dbo.Albums", "Name");
            DropColumn("dbo.Images", "Id");
            DropColumn("dbo.Images", "CreatedOn");
            DropColumn("dbo.Images", "Published");
            AddColumn("dbo.Albums", "AlbumId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Albums", "Title", c => c.String());
            AddColumn("dbo.Images", "ImageId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Images", "AddedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Images", "Visible", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.Albums", "AlbumId");
            AddPrimaryKey("dbo.Images", "ImageId");
            AddForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums", "AlbumId");

        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "Published", c => c.Boolean(nullable: false));
            AddColumn("dbo.Images", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Images", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Albums", "Name", c => c.String());
            AddColumn("dbo.Albums", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums");
            DropPrimaryKey("dbo.Images");
            DropPrimaryKey("dbo.Albums");
            DropColumn("dbo.Images", "Visible");
            DropColumn("dbo.Images", "AddedOn");
            DropColumn("dbo.Images", "ImageId");
            DropColumn("dbo.Albums", "Title");
            DropColumn("dbo.Albums", "AlbumId");
            AddPrimaryKey("dbo.Images", "Id");
            AddPrimaryKey("dbo.Albums", "Id");
            RenameIndex(table: "dbo.Images", name: "IX_Album_AlbumId", newName: "IX_Album_Id");
            RenameColumn(table: "dbo.Images", name: "Album_AlbumId", newName: "Album_Id");
            AddForeignKey("dbo.Images", "Album_Id", "dbo.Albums", "Id");
        }
    }
}
