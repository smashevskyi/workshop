using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using MyWorkshop.DAL.Abstract;
using System.Linq.Expressions;

namespace MyWorkshop.Controllers
{
    public class ThoughtsController : Controller
    {
        private IUnitOfWork unitOfWork;
        public ThoughtsController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        int pageSize = 3;

        public ActionResult List(string searchString, string tagName, int page = 1)
        {
            page = page < 1 ? 1 : page;

            IEnumerable<Post> result;
            PagingInfo pagingInfo;
            int totalItems;

            if (!String.IsNullOrEmpty(tagName))
            {

                Tag tag = unitOfWork.PostRepository.GetTagByName(tagName);
                if (tag == null)
                {

                    return new HttpNotFoundResult();

                }
                var temp = unitOfWork.PostRepository.GetPostByTagId(tag.Id);
                totalItems = temp.Count();
                result = temp
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            }
            else if (!String.IsNullOrWhiteSpace(searchString))
            {
                Expression<Func<Post, bool>> expression = (Post d) => (d.Description.Contains(searchString) || d.Title.Contains(searchString)) && d.Published;
                result = unitOfWork.PostRepository.GetPosts(expression, null, page, pageSize);
                totalItems = unitOfWork.PostRepository.CountPosts(filter: expression);
            }
            else
            {
                result = unitOfWork.PostRepository.GetPosts(page: page, size: pageSize);
                totalItems = unitOfWork.PostRepository.CountPosts(null);
            }

            foreach (var post in result)
            {
                var content = post.Description;
                int index = content.IndexOf("<p>", content.IndexOf("<p>") + 1);
                if (index != -1)
                {
                    post.Description = content.Substring(0, index);
                }
            }

            pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            };

            PostListViewModel model = new PostListViewModel
            {
                Posts = result,
                PagingInfo = pagingInfo
            };

            ViewBag.searchString = searchString;
            ViewBag.tagName = tagName;

            return View(model);
        }

        // GET: Thoughts/View/5
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = unitOfWork.PostRepository.GetPostById((int)id);
            if (post == null || post.Published == false)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult Archive(int year, int month, int page = 1)
        {
            page = page < 1 ? 1 : page;

            IEnumerable<Post> result;
            PagingInfo pagingInfo;
            int totalItems;

            Expression<Func<Post, bool>> expression = (Post p) => (p.Published && p.PostedOn.Year == year && p.PostedOn.Month == month);
            result = unitOfWork.PostRepository.GetPosts(filter: expression);
            totalItems = unitOfWork.PostRepository.CountPosts(expression);

            foreach (var post in result)
            {
                var content = post.Description;
                int index = content.IndexOf("<p>", content.IndexOf("<p>") + 1);
                if (index != -1)
                {
                    post.Description = content.Substring(0, index);
                }
            }

            pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            };

            PostListViewModel model = new PostListViewModel
            {
                Posts = result,
                PagingInfo = pagingInfo
            };

            ViewBag.IsArchive = true;
            ViewBag.Year = year;
            ViewBag.Month = month;

            return View("List", model);
        }


        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            SidebarComplexModel model = new SidebarComplexModel();

            var tags = unitOfWork.PostRepository.GetTagsCounted();

            if (tags.Count > 0)
            {
                int max = tags.Values.Max();
            }

            ViewBag.TagDictionary = tags;

            var posts = unitOfWork.PostRepository.GetPosts(p => p.Published, null, 1, 4);
            model.Posts = posts;

            var archiveEntry = unitOfWork.PostRepository.GetArchive();
            model.ArchiveEntries = archiveEntry;

            return PartialView("_Sidebar", model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
