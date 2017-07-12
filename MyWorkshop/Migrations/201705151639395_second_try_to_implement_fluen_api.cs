namespace MyWorkshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second_try_to_implement_fluen_api : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TagPosts", newName: "PostTag");
            DropForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums");
            DropIndex("dbo.Images", new[] { "Album_AlbumId" });
            RenameColumn(table: "dbo.PostTag", name: "Tag_Id", newName: "TagId");
            RenameColumn(table: "dbo.PostTag", name: "Post_Id", newName: "PostId");
            RenameIndex(table: "dbo.PostTag", name: "IX_Post_Id", newName: "IX_PostId");
            RenameIndex(table: "dbo.PostTag", name: "IX_Tag_Id", newName: "IX_TagId");
            DropPrimaryKey("dbo.PostTag");
            AlterColumn("dbo.Albums", "Title", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Albums", "Slug", c => c.String(nullable: false, maxLength: 44));
            AlterColumn("dbo.Albums", "Description", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Albums", "Occasion", c => c.String(maxLength: 24));
            AlterColumn("dbo.Images", "Name", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Images", "Album_AlbumId", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Title", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Posts", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "UrlSlug", c => c.String(nullable: false, maxLength: 48));
            AlterColumn("dbo.Tags", "Name", c => c.String(maxLength: 16));
            AddPrimaryKey("dbo.PostTag", new[] { "PostId", "TagId" });
            CreateIndex("dbo.Images", "Album_AlbumId");
            AddForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums", "AlbumId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums");
            DropIndex("dbo.Images", new[] { "Album_AlbumId" });
            DropPrimaryKey("dbo.PostTag");
            AlterColumn("dbo.Tags", "Name", c => c.String());
            AlterColumn("dbo.Posts", "UrlSlug", c => c.String());
            AlterColumn("dbo.Posts", "Description", c => c.String());
            AlterColumn("dbo.Posts", "Title", c => c.String());
            AlterColumn("dbo.Images", "Album_AlbumId", c => c.Int());
            AlterColumn("dbo.Images", "Name", c => c.String());
            AlterColumn("dbo.Albums", "Occasion", c => c.String());
            AlterColumn("dbo.Albums", "Description", c => c.String());
            AlterColumn("dbo.Albums", "Slug", c => c.String());
            AlterColumn("dbo.Albums", "Title", c => c.String());
            AddPrimaryKey("dbo.PostTag", new[] { "Tag_Id", "Post_Id" });
            RenameIndex(table: "dbo.PostTag", name: "IX_TagId", newName: "IX_Tag_Id");
            RenameIndex(table: "dbo.PostTag", name: "IX_PostId", newName: "IX_Post_Id");
            RenameColumn(table: "dbo.PostTag", name: "PostId", newName: "Post_Id");
            RenameColumn(table: "dbo.PostTag", name: "TagId", newName: "Tag_Id");
            CreateIndex("dbo.Images", "Album_AlbumId");
            AddForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums", "AlbumId");
            RenameTable(name: "dbo.PostTag", newName: "TagPosts");
        }
    }
}
