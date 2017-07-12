using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWorkshop.Controllers
{
    public class HomeController : Controller
    {
        private MWContext db;
        public HomeController()
        {
            db = new MWContext();
        }

        // GET: Home
        public ActionResult Index()
        {
            ManageViewModel model = new ManageViewModel();

            model.Posts = db.Posts
                .Where(p => p.Published)
                .OrderByDescending(p => p.Id)
                .Take(3)
                .ToList();

            foreach (var post in model.Posts)
            {
                var content = post.Description;
                int index = content.IndexOf("<p>", content.IndexOf("<p>") + 1);
                if (index != -1)
                {
                    post.Description = content.Substring(0, index);
                }
            }

            model.Albums = db.Albums
                .Where(a => a.Published)
                .OrderByDescending(a => a.AlbumId)
                .Take(4)
                .ToList();

            return View(model);
        }
    }
}