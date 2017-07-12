using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWorkshop.Areas.Manage.Controllers
{
    public class HomeController : Controller
    {
        private MWContext context;
        public HomeController()
        {
            context = new MWContext();
        }
        // GET: Manage/Index
        public ActionResult Index()
        {
            ManageViewModel model = new ManageViewModel();

            model.Posts = context.Posts.OrderByDescending(p => p.Id).Take(3).ToList();
            model.Albums = context.Albums.OrderByDescending(p => p.AlbumId).Take(4).ToList();

            return View(model);
        }
    }
}