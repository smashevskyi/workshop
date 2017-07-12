using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyWorkshop.Controllers
{
    public class ThoughtsController : Controller
    {
        private MWContext db;
        public ThoughtsController()
        {
            db = new MWContext();
        }
         
        int pageSize = 3;

        public ActionResult List(string searchString, string tagName, int page = 1)
        {
            page = page < 1 ? 1 : page;

            List<Post> result;
            PagingInfo pagingInfo;
            int totalItems;

            if (!String.IsNullOrEmpty(tagName))
            {
                try
                {
                    var temp = db.Tags
                                .Include(x => x.Posts)
                                .FirstOrDefault(x => x.Name == tagName.ToUpper())
                                .Posts.Where(p => p.Published).OrderByDescending(p => p.Id);

                    totalItems = temp.Count();

                    result = temp
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
                catch
                {
                    return new HttpNotFoundResult();
                }
            }
            else if (!String.IsNullOrWhiteSpace(searchString))
            {
                var posts = from p in db.Posts
                            select p;

                var temp = posts
                    .Where(d => (d.Description.Contains(searchString) || d.Title.Contains(searchString)) && d.Published)
                    .OrderBy(p => p.Id);

                totalItems = temp.Count();

                result = temp
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                var posts = from p in db.Posts
                            select p;

                var temp = posts
                    .Where(p => p.Published)
                    .OrderByDescending(p => p.Id);

                totalItems = temp.Count();

                result = temp
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
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
            Post post = db.Posts.Find(id);
            if (post == null || post.Published == false)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult Archive(int year, int month, int page = 1)
        {
            page = page < 1 ? 1 : page;

            List<Post> result;
            PagingInfo pagingInfo;
            int totalItems;

            var posts = from p in db.Posts
                        select p;

            var temp = posts
                .Where(p => p.Published && p.PostedOn.Year == year && p.PostedOn.Month == month)
                .OrderByDescending(p => p.Id);

            totalItems = temp.Count();

            result = temp
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

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

            var t = db.Tags
                .Include(x => x.Posts)
                .Where(x => x.Posts.Count > 0)
                .OrderBy(x => x.Name)
                .ToDictionary(x => x.Name, x => x.Posts.Count);

            if (t.Count > 0)
            {
                int max = t.Values.Max();
            }

            ViewBag.TagDictionary = t;

            var posts = db.Posts
                .Where(p => p.Published)
                .OrderByDescending(p => p.Id)
                .Take(4)
                .ToList();

            model.Posts = posts;

            var archiveEntry = db.Posts
                    .GroupBy(o => new
                    {
                        Month = o.PostedOn.Month,
                        Year = o.PostedOn.Year
                    })
                    .Select(g => new ArchiveEntry
                    {
                        Month = g.Key.Month,
                        Year = g.Key.Year,
                        Total = g.Count()
                    })
                    .OrderByDescending(a => a.Year)
                    .ThenByDescending(a => a.Month)
                    .ToList();

            model.ArchiveEntries = archiveEntry;

            return PartialView("_Sidebar", model);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
