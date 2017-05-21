using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.ImagesRecent = ImageVM.GetAllImages();
            if (User.Identity.IsAuthenticated)
            {
                List<ImageVM> imagesOfSubscription = new List<ImageVM>();
                foreach (var subscription in UserVM.GetSubscribtionsByUserId(UserVM.GetUserIdByNickname(User.Identity.Name)))
                {
                    imagesOfSubscription.AddRange(ImageVM.GetImagesByUserId(subscription.Id));
                }
                ViewBag.ImagesOfSubscriptions = imagesOfSubscription;
            }
            return View(ImageVM.GetAllImages());
        }

        [Authorize]
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Search(string searchQuery)
        {
            List<ImageVM> resultImages;
            resultImages = ImageVM.GetAllImages().Where(image => image.Description.ToLower().Contains(searchQuery.ToLower())).OrderBy(image => image.DateOfCreating).ToList();
            return View(resultImages);
        }
    }
}