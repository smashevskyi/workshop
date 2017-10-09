using System;
using Moq;
using MyWorkshop.Models;
using System.Collections.Generic;
using MyWorkshop.DAL.Abstract;
using System.Linq;
using MyWorkshop.Controllers;
using System.Web.Mvc;
using NUnit.Framework;
using MyWorkshop.ViewModels;

namespace MyWorkshop2.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {
        private IEnumerable<Post> posts;
        private IEnumerable<Album> albums;
        private Mock<IPostRepository> postMock;
        private Mock<IAlbumRepository> albumMock;
        private Mock<IUnitOfWork> mock;

        private HomeController controller;
        private ViewResult result;
        private ManageViewModel model;

        [SetUp]
        public void Setup()
        {

            posts = new List<Post>()
            {
                new Post() { Id = 0, Description = "<p>First par in post with id 0</p><p>Second par in post with id 0</p>", Title = "Zero" },
                new Post() { Id = 1, Description = "<p>First par in post with id 1</p><p>Second par in post with id 1</p>", Title = "First" },
                new Post() { Id = 2, Description = "<p>First par in post with id 2</p><p>Second par in post with id 2</p>", Title = "Second" },
                new Post() { Id = 3, Description = "<p>First par in post with id 3</p><p>Second par in post with id 3</p>", Title = "Third" }
            };

            albums = new List<Album>()
            {
                new Album() { AlbumId = 0, Title = "Zero", Description = "0.jpg", Slug = "0-zero", CreatedOn = new DateTime(2017, 01, 25), ModifiedOn = new DateTime(2017, 01, 25), Published = true, PreviewImagePath = "C:/0.jpg", },
                new Album() { AlbumId = 1, Title = "First", Description = "1.jpg", Slug = "1-first", CreatedOn = new DateTime(2017, 01, 25), ModifiedOn = new DateTime(2017, 01, 25), Published = true, PreviewImagePath = "C:/1.jpg", },
                new Album() { AlbumId = 2, Title = "Second", Description = "2.jpg", Slug = "2-second", CreatedOn = new DateTime(2017, 01, 25), ModifiedOn = new DateTime(2017, 01, 25), Published = true, PreviewImagePath = "C:/2.jpg", },
                new Album() { AlbumId = 3, Title = "Third", Description = "3.jpg", Slug = "3-third", CreatedOn = new DateTime(2017, 01, 25), ModifiedOn = new DateTime(2017, 01, 25), Published = true, PreviewImagePath = "C:/3.jpg", },
                new Album() { AlbumId = 4, Title = "Fourth", Description = "4.jpg", Slug = "4-fourth", CreatedOn = new DateTime(2017, 01, 25), ModifiedOn = new DateTime(2017, 01, 25), Published = true, PreviewImagePath = "C:/4.jpg", }
            };

            postMock = new Mock<IPostRepository>();
            //postMock.Setup(x => x.GetPosts(It.IsAny<System.Linq.Expressions.Expression<Func<Post, bool>>>(),
            //                                It.IsAny<Func<IQueryable<Post>, IOrderedQueryable<Post>>>(),
            //                                It.IsAny<int>(),
            //                                It.IsAny<int>()))
            //    .Returns(posts.AsQueryable());
            postMock.Setup(x => x.GetPosts(It.IsAny<System.Linq.Expressions.Expression<Func<Post, bool>>>(),
                                It.IsAny<Func<IQueryable<Post>, IOrderedQueryable<Post>>>(),
                                It.IsAny<int>(),
                                It.IsAny<int>()))
                    .Returns((System.Linq.Expressions.Expression<Func<Post, bool>> exp, Func<IQueryable<Post>, IOrderedQueryable<Post>> func, int page, int size) => posts.AsQueryable().Skip(page-1 * size).Take(size));

            albumMock = new Mock<IAlbumRepository>();
            //albumMock.Setup(x => x.GetAlbums(It.IsAny<System.Linq.Expressions.Expression<Func<Album, bool>>>(),
            //                                It.IsAny<Func<IQueryable<Album>, IOrderedQueryable<Album>>>(),
            //                                It.IsAny<bool>(),
            //                                It.IsAny<int>(),
            //                                It.IsAny<int>()))
            //    .Returns(albums.AsQueryable());
            albumMock.Setup(x => x.GetAlbums(It.IsAny<System.Linq.Expressions.Expression<Func<Album, bool>>>(),
                                It.IsAny<Func<IQueryable<Album>, IOrderedQueryable<Album>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<int>(),
                                It.IsAny<int>()))
                    .Returns((System.Linq.Expressions.Expression<Func<Album, bool>> exp, Func<IQueryable<Album>, IOrderedQueryable<Album>> func, bool published, int page, int size) => albums.AsQueryable().Skip(page-1 * size).Take(size));


            mock = new Mock<IUnitOfWork>();

            mock.Setup(x => x.PostRepository).Returns(postMock.Object);
            mock.Setup(x => x.AlbumRepository).Returns(albumMock.Object);

            controller = new HomeController(mock.Object);
            result = controller.Index() as ViewResult;
            model = result.ViewData.Model as ManageViewModel;
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void Index_ViewResult_NotNull()
        {
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Index_Model_NotNull()
        {
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        public void Index_ModelType_IsManageViewModel()
        {
            Assert.That(result.Model, Is.TypeOf(typeof(ManageViewModel)));
        }

        [Test]
        public void Index_PostsDescription_OnlyOneParagraph()
        {
            var post = model.Posts.FirstOrDefault();
            var description = post.Description;
            var count = System.Text.RegularExpressions.Regex.Matches(description, "<p>").Count;

            Assert.That(count, Is.EqualTo(1));
        }
        [Test]
        public void Index_PostsPaging_LessPostsThanInitialHas()
        {
            var initialCount = posts.Count();
            var count = model.Posts.Count();

            Assert.That(count, Is.LessThanOrEqualTo(initialCount));
        }

        [Test]
        public void Index_AlbumsPaging_LessAlbumsThanInitialHas()
        {
            var initialCount = albums.Count();
            var count = model.Albums.Count();

            Assert.That(count, Is.LessThanOrEqualTo(initialCount));
        }
    }
}
