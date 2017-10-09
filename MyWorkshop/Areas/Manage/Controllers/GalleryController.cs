using MyWorkshop.DAL.Abstract;
using MyWorkshop.Helpers;
using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyWorkshop.Areas.Manage.Controllers
{
    public class GalleryController : Controller
    {
        private IUnitOfWork unitOfWork;
        public GalleryController(IUnitOfWork uow)
        {
            unitOfWork = uow;
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

            var result = unitOfWork.AlbumRepository.GetAlbums(page:page, size:pageSize);

            ViewBag.Pages = Math.Ceiling((double)unitOfWork.AlbumRepository.CountAlbums() / pageSize);
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

                unitOfWork.AlbumRepository.InsertAlbum(album);
                unitOfWork.Save();

                // adding id to slug
                string idSlug = album.AlbumId + "-" + album.Slug;
                album.Slug = idSlug;
                unitOfWork.AlbumRepository.UpdateAlbum(album);
                unitOfWork.Save();

                // creating directories for images
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
            var album = unitOfWork.AlbumRepository.GetAlbumById(id);
            
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

            var album = unitOfWork.AlbumRepository.GetAlbumById((int)id);

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
            Album album = unitOfWork.AlbumRepository.GetAlbumById(id);

            if (ModelState.IsValid)
            {
                album.Title = viewAlbum.Title;
                album.Description = viewAlbum.Description;
                album.Occasion = viewAlbum.Occasion;
                album.Published = viewAlbum.Published;
                album.PreviewImagePath = viewAlbum.PreviewImagePath;
                unitOfWork.AlbumRepository.UpdateAlbum(album);
                unitOfWork.Save();
            }

            if (images != null)
            {
                foreach (var image in images)
                {
                    Image img = unitOfWork.AlbumRepository.GetImageById(image.ImageId);
                    img.Name = image.Name;
                    img.Visible = image.Visible;
                    unitOfWork.AlbumRepository.UpdateImage(img);                    
                }
                unitOfWork.Save();
            }

            return RedirectToAction("Album", new { id = album.AlbumId });
        }

        [HttpPost]
        public ActionResult UploadImages(List<ImageViewModel> images, int albumId)
        {
            if (Request.Files[0].ContentLength == 0)
            {
                return RedirectToAction("Album", new { id = albumId });
            }

            var album = unitOfWork.AlbumRepository.GetAlbumById(albumId);

            DateTime date = DateTime.Now;
            bool hasPreview = album.PreviewImagePath != null;
            int counter = album.Images.Count + 1;
            string albumPath = album.CreatedOn.Year + "/" + album.CreatedOn.ToString("MM") + "/" + album.Slug;
            string savePath = StorageRoot + albumPath;

            string fileName;

            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (ImageHelper.SaveToFolder(Request.Files[i], savePath, out fileName))
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
            unitOfWork.AlbumRepository.UpdateAlbum(album);
            unitOfWork.Save();

            return RedirectToAction("Album", new { id = albumId });
        }

        [HttpPost]
        public EmptyResult SortedList(List<string> items)
        {
            for(var i = 0; i < items.Count; i++)
            {
                int id = Convert.ToInt32(items[i]);
                Image image = unitOfWork.AlbumRepository.GetImageById(id);
                image.Position = i + 1;
                unitOfWork.AlbumRepository.UpdateImage(image);
            }
            unitOfWork.Save();
            return new EmptyResult();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteImage(int id)
        {
            Image imgToDel = unitOfWork.AlbumRepository.GetImageById(id);

            if(imgToDel == null)
            {
                return View();   
            }

            unitOfWork.AlbumRepository.DeleteImage(imgToDel);
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

            unitOfWork.Save();
            return View();
        }

        // когда удаляется изображение, которое так же является изобр. для превью, нужно выбрать новое, в данном случае берется первое из коллекции, или ничего если она пустая.
        public void RepickAlbumPreview(int id)
        {
            var album = unitOfWork.AlbumRepository.GetAlbumById(id);
            Image img = album.Images.FirstOrDefault();

            if(img != null)
            {
                album.PreviewImagePath = img.ImagePath;
            }
            else
            {
                album.PreviewImagePath = "";
            }

            unitOfWork.Save();
        }
    }
}