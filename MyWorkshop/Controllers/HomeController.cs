using MyWorkshop.DAL.Abstract;
using MyWorkshop.Models;
using MyWorkshop.ViewModels;
using System.Web.Mvc;

namespace MyWorkshop.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        // GET: Home
        public ActionResult Index()
        {
            ManageViewModel model = new ManageViewModel();

            model.Posts = unitOfWork.PostRepository.GetPosts((Post p) => p.Published, null, 1, 3);
            model.Albums = unitOfWork.AlbumRepository.GetAlbums((Album a) => a.Published, null, false, 1, 4);

            foreach (var post in model.Posts)
            {
                var content = post.Description;
                int index = content.IndexOf("<p>", content.IndexOf("<p>") + 1);
                if (index != -1)
                {
                    post.Description = content.Substring(0, index);
                }
            }

            return View(model);
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