using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyWorkshop.Models;
using MyWorkshop.DAL.Abstract;
using System.Web.Mvc;
using NUnit.Framework;
using MyWorkshop.ViewModels;
using MyWorkshop.Areas.Manage.Controllers;

namespace MyWorkshop2.Tests
{
    [TestFixture]
    public class AreaManagePostsController
    {
        private IEnumerable<Post> posts;
        //private IEnumerable<Tag> tags;
        private Mock<IPostRepository> postMock;
        private Mock<IUnitOfWork> mock;

        private PostsController controller;
        private ViewResult result;

        [SetUp]
        public void Setup()
        {
            Tag tag0 = new Tag() { Id = 0, Name = "TAG0" };
            Tag tag1 = new Tag() { Id = 1, Name = "TAG1" };
            Tag tag2 = new Tag() { Id = 2, Name = "TAG2" };
            Tag tag3 = new Tag() { Id = 3, Name = "TAG3" };

            posts = new List<Post>()
            {
                new Post() { Id = 0, Tags = new List<Tag>() { tag0, tag1 }, Description = "<p>First par in post with id 0</p><p>Second par in post with id 0</p>", Title = "Zero" },
                new Post() { Id = 1, Tags = new List<Tag>() { tag1, tag2 }, Description = "<p>First par in post with id 1</p><p>Second par in post with id 1</p>", Title = "First" },
                new Post() { Id = 2, Tags = new List<Tag>() { tag2, tag3 }, Description = "<p>First par in post with id 2</p><p>Second par in post with id 2</p>", Title = "Second" },
                new Post() { Id = 3, Tags = new List<Tag>() { tag3, tag0 }, Description = "<p>First par in post with id 3</p><p>Second par in post with id 3</p>", Title = "Third" }
            };
        }
    }
}
