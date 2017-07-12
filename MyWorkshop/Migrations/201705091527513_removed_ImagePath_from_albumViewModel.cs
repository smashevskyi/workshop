namespace MyWorkshop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removed_ImagePath_from_albumViewModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AlbumViewModels", "PreviewImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AlbumViewModels", "PreviewImagePath", c => c.String());
        }
    }
}
