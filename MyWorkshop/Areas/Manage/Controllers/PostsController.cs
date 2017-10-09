using MyWorkshop.DAL.Abstract;
using MyWorkshop.DAL.Concrete;
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
        private IUnitOfWork unitOfWork;
        public PostsController(IUnitOfWork uow)
        {
            unitOfWork = uow;
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
            var posts = unitOfWork.PostRepository.GetPosts(null, null, page, pageSize);

            ViewBag.Pages = Math.Ceiling((double)unitOfWork.PostRepository.GetPosts().Count() / pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;

            return View(posts);
        }


        // GET: Thoughts/View/5
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = unitOfWork.PostRepository.GetPostById((int)id);

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
                // upload image or download Bing daily picture
                if (image != null)
                {
                    string imgPath = DateTime.Now.ToString("yyyyMMddHHmm") + System.IO.Path.GetExtension(image.FileName);
                    string ImgSavePath = HttpContext.Server.MapPath("~/Content/Images/Thoughts/")
                                                          + imgPath;
                    try
                    {
                        image.SaveAs(ImgSavePath);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save image.");
                        return View(postView);
                    }

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
                        catch (Exception)
                        {
                            ModelState.AddModelError(string.Empty, "Can not download Bing image. Try again or upload existing image.");
                            return View(postView);
                        }

                    }
                } // end of image save if..else statement
                
                var post = new Post();
                post.Title = postView.Title;
                post.Description = postView.Description;
                post.PostedOn = DateTime.Now;
                post.Published = postView.Published;
                post.UrlSlug = SlugHelper.URLFriendly(postView.UrlSlug);
                post.ImagePath = postView.ImagePath;
                post.Tags = new List<Tag>();

                if (!String.IsNullOrWhiteSpace(postView.Tags))
                {
                    TagsToEntities(postView, post);
                }

                try
                {
                    post = unitOfWork.PostRepository.InsertPost(post);
                    unitOfWork.Save();
                    // Updating slug. Adding {id-} in the beginning
                    post.UrlSlug = post.Id + "-" + post.UrlSlug;
                    unitOfWork.PostRepository.UpdatePost(post);
                    unitOfWork.Save();
                }
                catch(Exception)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.");
                    return View(postView);
                }

                return RedirectToAction("Index", new { page = 1 });
            }

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

            Post post = unitOfWork.PostRepository.GetPostById((int)id);

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
                StringBuilder sb = new StringBuilder();
                foreach (var tag in post.Tags)
                {
                    sb.Append(tag.Name).Append(" ");
                }
                postView.Tags = sb.ToString().TrimEnd();
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
            var post = unitOfWork.PostRepository.GetPostById(postView.Id);

            if (ModelState.IsValid)
            {
                post.Title = postView.Title;
                post.Description = postView.Description;
                post.Published = postView.Published;

                if (image != null)
                {
                    string imgPath = DateTime.Now.ToString("yyyyMMddHHmm") + System.IO.Path.GetExtension(image.FileName);
                    string ImgSavePath = HttpContext.Server.MapPath("~/Content/Images/Thoughts/")
                                                          + imgPath;
                    try
                    {
                        image.SaveAs(ImgSavePath);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save image.");
                        return View(postView);
                    }

                    postView.ImagePath = imgPath;
                    post.ImagePath = postView.ImagePath;
                }

                post.Tags.Clear();

                if (!String.IsNullOrWhiteSpace(postView.Tags))
                {
                    TagsToEntities(postView, post);
                }
                try
                {
                    unitOfWork.PostRepository.UpdatePost(post);
                    unitOfWork.Save();
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.");
                    return View(postView);
                }
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

            Post post = unitOfWork.PostRepository.GetPostById((int)id);

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
            try
            {
                Post post = unitOfWork.PostRepository.GetPostById(id);
                unitOfWork.PostRepository.DeletePost(id);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                TempData["message"] = string.Format("Unable to delete post.");
                return RedirectToAction("Delete", new { id = id});
            }
            return RedirectToAction("Index");
        }

        // splits string of tags into array and insert them into DB if they are not exist
        private void TagsToEntities(PostViewModel postView, Post post)
        {
            postView.Tags = postView.Tags.Trim();
            var tagList = postView.Tags.Split(new Char[] { ' ' });

            foreach (var tag in tagList)
            {
                var tagInDb = unitOfWork.PostRepository.GetTagByName(tag.ToUpper());
                if (tagInDb != null)
                {
                    post.Tags.Add(tagInDb);
                }
                else
                {
                    post.Tags.Add(new Tag() { Name = tag.ToUpper() });
                }
            }
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