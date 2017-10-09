using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MyWorkshop.Models;

namespace MyWorkshop.DAL
{
    public class MWContext : DbContext
    {
        public MWContext() : base("MWContext")
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Book> Books { get; set; }

        //public System.Data.Entity.DbSet<MyWorkshop.ViewModels.AlbumViewModel> AlbumViewModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.HasDefaultSchema("Admin");

            modelBuilder.Entity<Album>().HasMany(a => a.Images).WithRequired(i => i.Album);

            modelBuilder.Entity<Post>().HasKey<int>(p => p.Id);
            modelBuilder.Entity<Tag>().HasKey<int>(t => t.Id);
            modelBuilder.Entity<Image>().HasKey<int>(i => i.ImageId);
            modelBuilder.Entity<Album>().HasKey<int>(a => a.AlbumId);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .Map(m =>
                {
                    m.ToTable("PostTag");
                    m.MapLeftKey("PostId");
                    m.MapRightKey("TagId");
                });

            //modelBuilder.Entity<Post>().Property(p => p.Id);
            modelBuilder.Entity<Post>().Property(p => p.Title).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Post>().Property(p => p.Description).IsRequired();
            modelBuilder.Entity<Post>().Property(p => p.UrlSlug).HasMaxLength(48).IsRequired();
            //modelBuilder.Entity<Post>().Property(p => p.Published);
            //modelBuilder.Entity<Post>().Property(p => p.PostedOn);
            //modelBuilder.Entity<Post>().Property(p => p.ImagePath);

            //modelBuilder.Entity<Tag>().Property(t => t.Id);
            modelBuilder.Entity<Tag>().Property(t => t.Name).HasMaxLength(16);

            //modelBuilder.Entity<Image>().Property(i => i.ImageId);
            modelBuilder.Entity<Image>().Property(i => i.Name).HasMaxLength(32).IsRequired();
            //modelBuilder.Entity<Image>().Property(i => i.Position);
            //modelBuilder.Entity<Image>().Property(i => i.AddedOn);
            //modelBuilder.Entity<Image>().Property(i => i.Visible);
            //modelBuilder.Entity<Image>().Property(i => i.ImagePath);
            //modelBuilder.Entity<Image>().Property(i => i.ThumbPath);
            //modelBuilder.Entity<Image>().Property(i => i.ResizedPath);


            //modelBuilder.Entity<Album>().Property(a => a.AlbumId);
            modelBuilder.Entity<Album>().Property(a => a.Title).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Album>().Property(a => a.Slug).HasMaxLength(44).IsRequired();
            modelBuilder.Entity<Album>().Property(a => a.Description).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<Album>().Property(a => a.Occasion).HasMaxLength(24);
            //modelBuilder.Entity<Album>().Property(a => a.Published);
            //modelBuilder.Entity<Album>().Property(a => a.CreatedOn);
            //modelBuilder.Entity<Album>().Property(a => a.ModifiedOn);
            //modelBuilder.Entity<Album>().Property(a => a.PreviewImagePath);

        }
    }
}