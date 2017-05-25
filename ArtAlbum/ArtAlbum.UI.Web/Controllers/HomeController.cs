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
            ViewBag.TopTags = ImageVM.GetAllTags().OrderByDescending(tagName => ImageVM.CountOfImagesWithTag(tagName)).Take(8);
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
        public ActionResult Search(string searchQuery, string tagsData, string userData,string radio)
        {
            if (radio == "image")
            {


                bool isSearchQueryEmpty = true;
                IEnumerable<ImageVM> resultImages = new List<ImageVM>();
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    isSearchQueryEmpty = false;
                    resultImages = ImageVM.GetAllImages().Where(image => image.Description.ToLower().Contains(searchQuery.ToLower())).OrderBy(image => image.DateOfCreating).ToList();
                }
                if (!string.IsNullOrWhiteSpace(tagsData))
                {
                    List<ImageVM> newImages = new List<ImageVM>();
                    if (isSearchQueryEmpty)
                    {
                        resultImages = ImageVM.GetAllImages().ToList();
                    }

                    foreach (var image in resultImages)
                    {
                        bool isTagExist = false;
                        foreach (var tag in tagsData.ToLower().Split(' '))
                        {
                            foreach (var imageTag in ImageVM.GetTagsByImage(image.Id))
                            {
                                if (tag == imageTag)
                                {
                                    isTagExist = true;
                                    break;
                                }
                                isTagExist = false;
                            }

                            if (!isTagExist)
                            {
                                break;
                            }
                        }

                        if (isTagExist)
                        {
                            newImages.Add(image);
                        }
                    }

                    resultImages = newImages;
                }
                return View(resultImages);
            }
            else if (radio == "user")
            {
                return View();
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult SearchUsers(string searchQuery)
        {
            IEnumerable<UserVM> resultUsers= new List<UserVM>();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                resultUsers = UserVM.GetAllUsers().Where(user => user.Nickname.ToLower().Contains(searchQuery.ToLower())).OrderBy(user => user.Nickname).ToList();
            }
            return View(resultUsers);
        }
    }
}