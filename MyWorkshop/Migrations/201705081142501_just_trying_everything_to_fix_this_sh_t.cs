namespace MyWorkshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class just_trying_everything_to_fix_this_sh_t : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PostTag", newName: "TagPosts");
            DropForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums");
            DropIndex("dbo.Images", new[] { "Album_AlbumId" });
            RenameColumn(table: "dbo.TagPosts", name: "PostId", newName: "Post_Id");
            RenameColumn(table: "dbo.TagPosts", name: "TagId", newName: "Tag_Id");
            RenameIndex(table: "dbo.TagPosts", name: "IX_TagId", newName: "IX_Tag_Id");
            RenameIndex(table: "dbo.TagPosts", name: "IX_PostId", newName: "IX_Post_Id");
            DropPrimaryKey("dbo.TagPosts");
            AlterColumn("dbo.Albums", "Title", c => c.String());
            AlterColumn("dbo.Albums", "Slug", c => c.String());
            AlterColumn("dbo.Albums", "Description", c => c.String());
            AlterColumn("dbo.Albums", "Occasion", c => c.String());
            AlterColumn("dbo.Images", "Name", c => c.String());
            AlterColumn("dbo.Images", "Album_AlbumId", c => c.Int());
            AlterColumn("dbo.Posts", "Title", c => c.String());
            AlterColumn("dbo.Posts", "Description", c => c.String());
            AlterColumn("dbo.Posts", "UrlSlug", c => c.String());
            AlterColumn("dbo.Tags", "Name", c => c.String());
            AddPrimaryKey("dbo.TagPosts", new[] { "Tag_Id", "Post_Id" });
            CreateIndex("dbo.Images", "Album_AlbumId");
            AddForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums", "AlbumId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums");
            DropIndex("dbo.Images", new[] { "Album_AlbumId" });
            DropPrimaryKey("dbo.TagPosts");
            AlterColumn("dbo.Tags", "Name", c => c.String(maxLength: 16));
            AlterColumn("dbo.Posts", "UrlSlug", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "Title", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Images", "Album_AlbumId", c => c.Int(nullable: false));
            AlterColumn("dbo.Images", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Albums", "Occasion", c => c.String(maxLength: 30));
            AlterColumn("dbo.Albums", "Description", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Albums", "Slug", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Albums", "Title", c => c.String(nullable: false, maxLength: 50));
            AddPrimaryKey("dbo.TagPosts", new[] { "PostId", "TagId" });
            RenameIndex(table: "dbo.TagPosts", name: "IX_Post_Id", newName: "IX_PostId");
            RenameIndex(table: "dbo.TagPosts", name: "IX_Tag_Id", newName: "IX_TagId");
            RenameColumn(table: "dbo.TagPosts", name: "Tag_Id", newName: "TagId");
            RenameColumn(table: "dbo.TagPosts", name: "Post_Id", newName: "PostId");
            CreateIndex("dbo.Images", "Album_AlbumId");
            AddForeignKey("dbo.Images", "Album_AlbumId", "dbo.Albums", "AlbumId", cascadeDelete: true);
            RenameTable(name: "dbo.TagPosts", newName: "PostTag");
        }
    }
}
