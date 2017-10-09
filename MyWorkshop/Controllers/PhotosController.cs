using System;
using System.Web.Mvc;
using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using System.Net;
using MyWorkshop.DAL.Abstract;
using System.Text.RegularExpressions;

namespace MyWorkshop.Controllers
{
    public class PhotosController : Controller
    {
        private IUnitOfWork unitOfWork;
        public PhotosController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        string serverMapPath = "~/Content/Images/Photos/";
        private string StorageRoot
        {
            get { return HttpContext.Server.MapPath(serverMapPath); }
        }

        // GET: Photos
        public ActionResult Index()
        {
            var firstDay = DateTime.Today.AddDays(-30);
            AlbumComplexModel model = new AlbumComplexModel();

            model.Albums = unitOfWork.AlbumRepository.GetAlbums(filter:(Album a) => (a.Published && a.CreatedOn >= firstDay), includeProp:true);
            model.ArchiveEntries = unitOfWork.AlbumRepository.GetArchive();

            return View(model);
        }

        public ActionResult Archive(int year, int month)
        {
            AlbumComplexModel model = new AlbumComplexModel();

            model.Albums = unitOfWork.AlbumRepository.GetAlbums(filter:(Album a) => (a.Published && a.CreatedOn.Year == year && a.CreatedOn.Month == month));
            model.ArchiveEntries = unitOfWork.AlbumRepository.GetArchive();

            return View("Index", model);
        }

        // Альбом запрашивается по url slug, но нам действительно нужен только айди который вшит в его начале.
        public ActionResult Album(string urlSlug)
        {
            int id;
            bool result = Int32.TryParse(Regex.Match(urlSlug, @"\d+").Value, out id);

            if (result)
            {
                Album album = unitOfWork.AlbumRepository.GetAlbumById(id);
                if (album == null || album.Published == false)
                {
                    return HttpNotFound();
                }
                return View(album);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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