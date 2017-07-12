using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Hosting;
using MyWorkshop.Helpers;
using System.Net;

namespace MyWorkshop.Controllers
{
    public class PhotosController : Controller
    {
        private MWContext db;
        public PhotosController()
        {
            db = new MWContext();
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

            List<Album> albums = db.Albums
                .Where(a => a.Published && a.CreatedOn >= firstDay)
                .OrderByDescending(a => a.AlbumId)
                .Include(a => a.Images)
                .ToList();

            model.Albums = albums;

            var archiveEntry = db.Albums
                .GroupBy(o => new
                {
                    Month = o.CreatedOn.Month,
                    Year = o.CreatedOn.Year
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

            return View(model);
        }

        public ActionResult Archive(int year, int month)
        {
            AlbumComplexModel model = new AlbumComplexModel();

            var albums = db.Albums
                .Where(a => a.Published && a.CreatedOn.Year == year && a.CreatedOn.Month == month)
                .OrderByDescending(a => a.AlbumId)
                .ToList();

            model.Albums = albums;

            var archiveEntry = db.Albums
                .GroupBy(o => new
                {
                    Month = o.CreatedOn.Month,
                    Year = o.CreatedOn.Year
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

            return View("Index", model);
        }

        public ActionResult Album(string urlSlug)
        {
            if (String.IsNullOrWhiteSpace(urlSlug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Album album = db.Albums
                            .Where(x => x.Slug == urlSlug.ToLower())
                            .Include(x => x.Images)
                            .FirstOrDefault();

            if (album == null || album.Published == false)
            {
                return HttpNotFound();
            }

            return View(album);
        }

    }
}