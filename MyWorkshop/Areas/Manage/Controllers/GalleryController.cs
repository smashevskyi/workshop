using MyWorkshop.Helpers;
using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyWorkshop.Areas.Manage.Controllers
{
    public class GalleryController : Controller
    {
        private MWContext context;
        public GalleryController()
        {
            context = new MWContext();
        }
        

        String serverMapPath = "~/Content/Images/Photos/";
        private string StorageRoot
        {
            get { return HttpContext.Server.MapPath(serverMapPath); }
        }

        // GET: Manage/Gallery
        public ActionResult Index(int page = 1)
        {
            int pageSize = 15;

            var albums = from a in context.Albums
                         orderby a.AlbumId descending
                         select a;

            var result = albums
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Pages = Math.Ceiling((double)albums.Count() / pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;

            return View(result);
        }

        [HttpGet]
        public ActionResult NewAlbum()
        {
            return PartialView("_CreateAlbum");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAlbum([Bind(Exclude = "AlbumId")] AlbumViewModel viewModel) // , HttpPostedFileBase image = null
        {
            if (ModelState.IsValid)
            {
                DateTime date = DateTime.Now;
                Album album = new Album();

                album.Title = viewModel.Title;
                album.Slug = SlugHelper.URLFriendly(viewModel.Slug);
                album.Description = viewModel.Description;
                album.Occasion = viewModel.Occasion;
                album.Published = viewModel.Published;
                album.CreatedOn = DateTime.Now;
                album.ModifiedOn = DateTime.Now;

                context.Albums.Add(album);
                context.SaveChanges();

                // adding id to slug
                context.Entry(album).GetDatabaseValues();
                string idSlug = album.AlbumId + "-" + album.Slug;
                context.Entry(album).Property(a => a.Slug).CurrentValue = idSlug;
                context.SaveChanges();

                // creating directories for image save
                string albumPath = date.Year + "/" + date.ToString("MM") + "/" + idSlug;
                string savePath = StorageRoot + albumPath;
                Directory.CreateDirectory(savePath + "/Original");
                Directory.CreateDirectory(savePath + "/Thumbs");
                Directory.CreateDirectory(savePath + "/Resized");

                ViewBag.AlbumId = album.AlbumId;
                return RedirectToAction("Album", new { id = album.AlbumId });
            }
            else
            {
                return PartialView("_CreateAlbum", viewModel);
            }
        }

        public ActionResult Album(int id)
        {
            var album = context.Albums.Where(x => x.AlbumId == id).Include(x => x.Images).FirstOrDefault();

            if (album == null)
            {
                return HttpNotFound();
            }

            album.Images = album.Images.OrderBy(i => i.Position).ToList();
            return View(album);
        }

        public ActionResult EditAlbum(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var album = context.Albums.Where(x => x.AlbumId == id).Include(x => x.Images).FirstOrDefault();

            if (album == null)
            {
                return HttpNotFound();
            }

            album.Images = album.Images.OrderBy(i => i.Position).ToList();
            return View(album);
        }


        [HttpPost]
        public ActionResult EditAlbum([Bind(Exclude = "CreatedOn, ModifiedOn, Slug")] Album viewAlbum, [Bind(Exclude="Position")] List<ImageViewModel> images)
        {
            var date = DateTime.Now;
            var id = viewAlbum.AlbumId;
            Album album = context.Albums.Find(id);

            if (ModelState.IsValid)
            {
                context.Entry(album).Property(u => u.Title).CurrentValue = viewAlbum.Title;
                context.Entry(album).Property(u => u.Description).CurrentValue = viewAlbum.Description;
                context.Entry(album).Property(u => u.Occasion).CurrentValue = viewAlbum.Occasion;
                context.Entry(album).Property(u => u.Published).CurrentValue = viewAlbum.Published;
                context.Entry(album).Property(u => u.PreviewImagePath).CurrentValue = viewAlbum.PreviewImagePath;
                context.SaveChanges();
            }


            if (images != null)
            {
                foreach (var model in images)
                {
                    Image img = context.Images.Find(model.ImageId);
                    context.Entry(img).Property(u => u.Name).CurrentValue = model.Name;
                    context.Entry(img).Property(u => u.Visible).CurrentValue = model.Visible;
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Album", new { id = album.AlbumId });

        }

        public ActionResult EditImages(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var album = context.Albums.Where(x => x.AlbumId == id).Include(x => x.Images).FirstOrDefault();

            if (album == null)
            {
                return HttpNotFound();
            }

            ViewBag.AlbumId = id;
            return View(album.Images.OrderBy(i => i.Position).ToList());
        }

        [HttpPost]
        public ActionResult EditImages(List<ImageViewModel> images, int albumId)
        {
            if (images != null)
            {
                foreach (var model in images)
                {
                    Image img = context.Images.Find(model.ImageId);
                    img.Name = model.Name;
                    img.Visible = model.Visible;
                    context.SaveChanges();
                }
            }

            return RedirectToAction("EditAlbum", new { id = albumId });
        }

        [HttpPost]
        public ActionResult UploadImages(List<ImageViewModel> images, int albumId)
        {
            if (Request.Files[0].ContentLength == 0)
            {
                ViewBag.AlbumId = albumId;
                return View("EditImages", context.Albums.Find(albumId).Images.ToList());
            }

            var album = context.Albums.Find(albumId);
            DateTime date = DateTime.Now;
            bool hasPreview = album.PreviewImagePath != null;
            int counter = album.Images.Count + 1;
            string albumPath = date.Year + "/" + date.ToString("MM") + "/" + album.Slug;
            string savePath = StorageRoot + albumPath;

            ImageHelper helper = new ImageHelper();
            string fileName;

            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (helper.SaveToFolder(Request.Files[i], savePath, out fileName)) // true - create sub dirs
                {
                    var img = new Image();
                    img.Name = Path.GetFileNameWithoutExtension(fileName);
                    img.Position = counter++;
                    img.Visible = true;
                    img.ImagePath = albumPath + "/Original/" + fileName;
                    img.ThumbPath = albumPath + "/Thumbs/" + fileName;
                    img.ResizedPath = albumPath + "/Resized/" + fileName;
                    img.AddedOn = date;

                    if (!hasPreview)
                    {
                        album.PreviewImagePath = img.ImagePath;
                        hasPreview = true;
                    }
                    album.Images.Add(img);
                }
            }
            context.SaveChanges();

            return RedirectToAction("Album", new { id = albumId });
        }

        [HttpPost]
        public EmptyResult SortedList(List<string> items)
        {
            for(var i = 0; i < items.Count; i++)
            {
                int id = Convert.ToInt32(items[i]);
                Image image = context.Images.Find(id);
                context.Entry(image).Property(img => img.Position).CurrentValue = i+1;
                context.SaveChanges();
            }
            return new EmptyResult();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteImage(int id)
        {
            Image imgToDel = context.Images.Find(id);
            context.Images.Remove(imgToDel);

            string imgPath = Path.Combine(StorageRoot, imgToDel.ImagePath);
            string thmbPath = Path.Combine(StorageRoot, imgToDel.ThumbPath);
            string resizePath = Path.Combine(StorageRoot, imgToDel.ResizedPath);

            if (System.IO.File.Exists(imgPath))
            {
                System.IO.File.Delete(imgPath);
            }
            if (System.IO.File.Exists(thmbPath))
            {
                System.IO.File.Delete(thmbPath);
            }
            if (System.IO.File.Exists(resizePath))
            {
                System.IO.File.Delete(resizePath);
            }
            context.SaveChanges();
            
            return View();
        }

        public void RepickAlbumPreview(int id)
        {
            var album = context.Albums.Find(id);
            Image img = album.Images.FirstOrDefault();
            if(img != null)
            {
                album.PreviewImagePath = img.ImagePath;
            }
            else
            {
                album.PreviewImagePath = "";
            }
            context.SaveChanges();
        }
    }
}