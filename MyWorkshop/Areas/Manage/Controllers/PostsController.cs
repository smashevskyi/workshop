using MyWorkshop.Helpers;
using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyWorkshop.Areas.Manage.Controllers
{
    public class PostsController : Controller
    {
        private MWContext context;
        public PostsController()
        {
            context = new MWContext();
        }

        String serverMapPath = "~/Content/Images/Photos/";
        private string StorageRoot
        {
            get { return HttpContext.Server.MapPath(serverMapPath); }
        }

        // GET: Manage/Posts
        public ActionResult Index(int page = 1)
        {
            int pageSize = 15;

            var posts = from p in context.Posts
                        orderby p.Id descending
                        select p;

            var result = posts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Pages = Math.Ceiling((double)posts.Count() / pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;

            return View(result);
        }


        // GET: Thoughts/View/5
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = context.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Thoughts/Create
        public ActionResult Create()
        {
            var postView = new PostViewModel();
            ViewBag.Title = "Create";
            return View("CreateEdit", postView);
        }

        // POST: Thoughts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PostViewModel postView, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var post = new Post();
                post.Title = postView.Title;
                post.Description = postView.Description;
                post.PostedOn = DateTime.Now;
                post.Published = postView.Published;
                post.UrlSlug = SlugHelper.URLFriendly(postView.UrlSlug);

                // upload image or download Bing daily picture
                if (image != null)
                {
                    string imgPath = DateTime.Now.ToString("yyyyMMddHHmm") + System.IO.Path.GetExtension(image.FileName);
                    string ImgSavePath = HttpContext.Server.MapPath("~/Content/Images/Thoughts/")
                                                          + imgPath;
                    image.SaveAs(ImgSavePath);
                    postView.ImagePath = imgPath;
                }
                else
                {
                    using (var webClient = new WebClient())
                    {
                        string alterPath = "";
                        try
                        {
                            var json = webClient.DownloadString("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US");
                            JObject jResults = JObject.Parse(json);
                            foreach (var img in jResults["images"])
                            {
                                alterPath = ((string)img["url"]);
                            }

                            string imgPath = DateTime.Now.ToString("yyyyMMddHHmm") + ".jpg";
                            string ImgSavePath = HttpContext.Server.MapPath("~/Content/Images/Thoughts/") + imgPath;
                            webClient.DownloadFile("https://www.bing.com/" + alterPath, ImgSavePath);
                            postView.ImagePath = imgPath;
                        }
                        catch(Exception)
                        {
                            ModelState.AddModelError(string.Empty, "Can not download Bing image. Try again or upload existing image.");
                            return View(postView);
                        }
 
                    }
                } // end of image save if..else statement

                post.ImagePath = postView.ImagePath;
                post.Tags = new List<Tag>();
                
                if (!String.IsNullOrWhiteSpace(postView.Tags))
                {
                    postView.Tags = postView.Tags.Trim();
                    var tagList = postView.Tags.Split(new Char[] { ' ' });

                    foreach (var tag in tagList)
                    {
                        var tagInDb = context.Tags.Where(t => t.Name == tag.ToUpper()).SingleOrDefault();
                        if (tagInDb != null)
                        {
                            post.Tags.Add(tagInDb);
                        }
                        else
                        {
                            var objTag = context.Tags.Add(new Tag() { Name = tag.ToUpper() });
                            context.SaveChanges();
                            post.Tags.Add(objTag);
                        }
                    }
                }

                context.Posts.Add(post);
                context.SaveChanges();

                // Updating slug. Adding {id-} in the beginning
                context.Entry(post).GetDatabaseValues();
                string idSlug = post.Id + "-" + post.UrlSlug;
                context.Entry(post).Property(u => u.UrlSlug).CurrentValue = idSlug;
                context.SaveChanges();

                return RedirectToAction("Index", new { page = 1 });
            } // end of block "if model is valid"

            ViewBag.Title = "Create";
            return View("CreateEdit", postView);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = context.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            PostViewModel postView = new PostViewModel();
            postView.Id = post.Id;
            postView.Title = post.Title;
            postView.Description = post.Description;
            postView.Published = post.Published;
            postView.ImagePath = post.ImagePath;
            postView.UrlSlug = post.UrlSlug;

            if (post.Tags != null && post.Tags.Count > 0)
            {
                foreach (var tag in post.Tags)
                {
                    postView.Tags += tag.Name + " ";
                }
                postView.Tags = postView.Tags.Trim();
            }

            ViewBag.Title = "Edit";
            return View("CreateEdit", postView);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PostViewModel postView, HttpPostedFileBase image = null)
        {
            var post = context.Posts.Find(postView.Id);

            if (ModelState.IsValid)
            {
                context.Entry(post).Property(u => u.Title).CurrentValue = postView.Title;
                context.Entry(post).Property(u => u.Description).CurrentValue = postView.Description;
                context.Entry(post).Property(u => u.Published).CurrentValue = postView.Published;

                if (image != null)
                {
                    string imgPath = DateTime.Now.ToString("yyyyMMddHHmm") + System.IO.Path.GetExtension(image.FileName);
                    string ImgSavePath = HttpContext.Server.MapPath("~/Content/Images/Thoughts/")
                                                          + imgPath;
                    image.SaveAs(ImgSavePath);
                    postView.ImagePath = imgPath;
                    context.Entry(post).Property(u => u.ImagePath).CurrentValue = postView.ImagePath;
                }

                post.Tags.Clear();

                if (!String.IsNullOrWhiteSpace(postView.Tags))
                {
                    var tagList = postView.Tags.Split(new Char[] { ' ' });

                    foreach (var tag in tagList)
                    {
                        var tagInDb = context.Tags.Where(t => t.Name == tag.ToUpper()).SingleOrDefault();
                        if (tagInDb != null)
                        {
                            post.Tags.Add(tagInDb);
                        }
                        else
                        {
                            var objTag = context.Tags.Add(new Tag() { Name = tag.ToUpper() });
                            context.SaveChanges();
                            post.Tags.Add(objTag);
                        }
                        context.SaveChanges();
                    }
                }

                context.SaveChanges();
                TempData["message"] = string.Format("Post has been saved");
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Edit";
            return View("CreateEdit", postView);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = context.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = context.Posts.Find(id);
            context.Posts.Remove(post);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}