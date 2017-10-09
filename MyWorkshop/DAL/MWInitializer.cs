using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWorkshop.Models;

namespace MyWorkshop.DAL
{
    public class MWInitializer : System.Data.Entity.DropCreateDatabaseAlways<MWContext>
    {
        protected override void Seed(MWContext context)
        {
            var img1 = new Image()
            {
                Name = "First image",
                Position = 1,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342555439430128.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342555439430128.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342555439430128.jpg",
                Visible = true
            };
            var img2 = new Image()
            {
                Name = "Second image",
                Position = 2,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342555445974574.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342555445974574.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342555445974574.jpg",
                Visible = true
            };
            var img3 = new Image()
            {
                Name = "Trd image",
                Position = 3,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342555455088788.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342555455088788.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342555455088788.jpg",
                Visible = true
            };
            var img4 = new Image()
            {
                Name = "Trd image",
                Position = 4,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342555547920570.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342555547920570.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342555547920570.jpg",
                Visible = true
            };
            var img5 = new Image()
            {
                Name = "Trd image",
                Position = 5,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342555555645712.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342555555645712.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342555555645712.jpg",
                Visible = true
            };
            var img6 = new Image()
            {
                Name = "Trd image",
                Position = 6,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342555725336549.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342555725336549.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342555725336549.jpg",
                Visible = true
            };
            var img7 = new Image()
            {
                Name = "Trd image",
                Position = 7,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342555759671530.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342555759671530.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342555759671530.jpg",
                Visible = true
            };
            var img8 = new Image()
            {
                Name = "Trd image",
                Position = 8,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342555905681855.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342555905681855.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342555905681855.jpg",
                Visible = true
            };
            var img9 = new Image()
            {
                Name = "Trd image",
                Position = 9,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342555940180068.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342555940180068.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342555940180068.jpg",
                Visible = true
            };
            var img10 = new Image()
            {
                Name = "Trd image",
                Position = 10,
                AddedOn = DateTime.Now,
                ImagePath = "2017/04/1-first-album/Original/636342556071457397.jpg",
                ThumbPath = "2017/04/1-first-album/Thumbs/636342556071457397.jpg",
                ResizedPath = "2017/04/1-first-album/Resized/636342556071457397.jpg",
                Visible = true
            };


            var album = new Album()
            {
                Title = "Album name sample",
                CreatedOn = DateTime.Parse("01/04/2017"),
                Description = "This is some basic description",
                Published = true,
                ModifiedOn = DateTime.Now,
                Occasion = "Test",
                PreviewImagePath = "2017/04/1-first-album/Original/636342555455088788.jpg",
                Slug = "1-first-album",
            };
            album.Images.Add(img1);
            album.Images.Add(img2);
            album.Images.Add(img3);
            album.Images.Add(img4);
            album.Images.Add(img5);
            album.Images.Add(img6);
            album.Images.Add(img7);
            album.Images.Add(img8);
            album.Images.Add(img9);
            album.Images.Add(img10);

            context.Albums.Add(album);
            context.SaveChanges();

            var post1 = new Post()
            {
                Title = "Title of 1",
                UrlSlug = "1_title_of_1",
                Description = "<p>Lorem Ipsum - это текст-'рыба', часто используемый в печати и вэб-дизайне.</p><p>Lorem Ipsum является стандартной 'рыбой' для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн. Его популяризации в новое время послужили публикация листов Letraset с образцами Lorem Ipsum в 60-х годах и, в более недавнее время, программы электронной вёрстки типа Aldus PageMaker, в шаблонах которых используется Lorem Ipsum.</p>",
                Tags = new List<Tag>(),
                PostedOn = DateTime.Now,
                Published = true,
                ImagePath = "8.jpg"
            };
            var post2 = new Post() { Title = "Title of 2", ImagePath = "8.jpg", UrlSlug = "2_slug2", Description = "<p>Lorem Ipsum - это текст-'рыба', часто используемый в печати и вэб-дизайне.</p><p>Lorem Ipsum является стандартной 'рыбой' для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн. Его популяризации в новое время послужили публикация листов Letraset с образцами Lorem Ipsum в 60-х годах и, в более недавнее время, программы электронной вёрстки типа Aldus PageMaker, в шаблонах которых используется Lorem Ipsum.</p>", Tags = new List<Tag>(), PostedOn = DateTime.Now, Published = true };
            var post3 = new Post() { Title = "Title of 3", ImagePath = "8.jpg", UrlSlug = "3_slug3", Description = "3Some long text3", Tags = new List<Tag>(), PostedOn = DateTime.Now, Published = true };
            var post4 = new Post() { Title = "Title of 4", ImagePath = "8.jpg", UrlSlug = "4_slug4", Description = "<p>Lorem Ipsum - это текст-'рыба', часто используемый в печати и вэб-дизайне.</p><p>Lorem Ipsum является стандартной 'рыбой' для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн. Его популяризации в новое время послужили публикация листов Letraset с образцами Lorem Ipsum в 60-х годах и, в более недавнее время, программы электронной вёрстки типа Aldus PageMaker, в шаблонах которых используется Lorem Ipsum.</p>", Tags = new List<Tag>(), PostedOn = DateTime.Now, Published = true };
            var post5 = new Post()
            {
                Title = "Title of 5",
                UrlSlug = "5_title_of_5",
                Description = "<p>Lorem Ipsum - это текст-'рыба', часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной 'рыбой' для текстов на латинице с начала XVI века.</p><p>В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн. Его популяризации в новое время послужили публикация листов Letraset с образцами Lorem Ipsum в 60-х годах и, в более недавнее время, программы электронной вёрстки типа Aldus PageMaker, в шаблонах которых используется Lorem Ipsum.</p>",
                Tags = new List<Tag>(),
                PostedOn = DateTime.Now,
                Published = true,
                ImagePath = "8.jpg"
            };
            var post6 = new Post()
            {
                Title = "Title of 6",
                UrlSlug = "6_title_of_6",
                Description = "<p>Lorem Ipsum - это текст-'рыба', часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной 'рыбой' для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн. Его популяризации в новое время послужили публикация листов Letraset с образцами Lorem Ipsum в 60-х годах и, в более недавнее время, программы электронной вёрстки типа Aldus PageMaker, в шаблонах которых используется Lorem Ipsum.</p>",
                Tags = new List<Tag>(),
                PostedOn = DateTime.Now,
                Published = false,
                ImagePath = "8.jpg"
            };

            var tag1 = new Tag() { Name = "FIRST" };
            var tag2 = new Tag() { Name = "SECOND" };
            var tag3 = new Tag() { Name = "THIRD" };
            var tag4 = new Tag() { Name = "FOURTH" };

            post1.Tags.Add(tag1);

            post2.Tags.Add(tag1);
            post2.Tags.Add(tag2);

            post3.Tags.Add(tag1);
            post3.Tags.Add(tag2);
            post3.Tags.Add(tag3);

            post4.Tags.Add(tag1);
            post4.Tags.Add(tag2);
            post4.Tags.Add(tag3);
            post4.Tags.Add(tag4);

            post5.Tags.Add(tag2);

            post6.Tags.Add(tag4);

            context.Posts.Add(post1);
            context.Posts.Add(post2);
            context.Posts.Add(post3);
            context.Posts.Add(post4);
            context.Posts.Add(post5);
            context.Posts.Add(post6);

            context.SaveChanges();



            context.Books.Add(new Book() { Title = "Defender Of Rainbows", Author = "Jacqueline J. Sharp", Year = 2015, Cover = "Paperback" });
            context.Books.Add(new Book() { Title = "Descendants Without Glory", Author = "Sebastian Harrison", Year = 2012, Cover = "Hard" });
            context.Books.Add(new Book() { Title = "Hunted By My Family", Author = "Amy Patel", Year = 2014, Cover = "Hard" });
            context.Books.Add(new Book() { Title = "Limits Of Electricity", Author = "Ewan Lawson", Year = 2011, Cover = "Paperback" });
            context.Books.Add(new Book() { Title = "Maverik The Parrot", Author = "Logan Paul", Year = 2017, Cover = "Savage" });
            context.SaveChanges();
        }
    }
}