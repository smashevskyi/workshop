using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWorkshop.Controllers
{
    public class WorkshopController : Controller
    {
        // GET: Workshop
        public ActionResult Index()
        {
            return View();
        }
    }
}