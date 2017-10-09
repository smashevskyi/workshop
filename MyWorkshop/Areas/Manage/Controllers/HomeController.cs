using MyWorkshop.DAL.Abstract;
using MyWorkshop.ViewModels;
using System.Web.Mvc;

namespace MyWorkshop.Areas.Manage.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }
        
        // GET: Manage/Index
        public ActionResult Index()
        {
            ManageViewModel model = new ManageViewModel();

            // model.Posts = context.Posts.OrderByDescending(p => p.Id).Take(3).ToList();
            model.Posts = unitOfWork.PostRepository.GetPosts(null, null, 1, 3);
            model.Albums = unitOfWork.AlbumRepository.GetAlbums(page: 1, size: 4);

            return View(model);
        }
    }
}