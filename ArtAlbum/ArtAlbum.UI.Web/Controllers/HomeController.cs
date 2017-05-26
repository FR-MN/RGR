﻿using ArtAlbum.UI.Web.Models;
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
            ViewBag.ImagesRecentPart = ImageVM.GetRecentImages(30);
            ViewBag.TopTags = ImageVM.GetAllTags().OrderByDescending(tagName => ImageVM.CountOfImagesWithTag(tagName)).Take(9);
            if (User.Identity.IsAuthenticated)
            {
                List<ImageVM> imagesOfSubscription = new List<ImageVM>();
                foreach (var subscription in UserVM.GetSubscribtionsByUserId(UserVM.GetUserIdByNickname(User.Identity.Name)))
                {
                    imagesOfSubscription.AddRange(ImageVM.GetImagesByUserId(subscription.Id));
                }
                ViewBag.ImagesOfSubscriptions = imagesOfSubscription;
            }
            return View();
        }


        [Authorize]
        [HttpGet]
        public ActionResult Search(string searchQuery, string tagsData, string userData, string typeOfRequest)
        {
            if (typeOfRequest == "images")
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
                ViewBag.ImagesSearch = resultImages;
            }
            else if (typeOfRequest == "users")
            {
                IEnumerable<UserVM> resultUsers = new List<UserVM>();
                if (!string.IsNullOrWhiteSpace(userData))
                {
                    resultUsers = UserVM.GetAllUsers().Where(user => user.Nickname.ToLower().Contains(userData.ToLower())).OrderBy(user => user.Nickname).ToList();
                }
                ViewBag.UsersSearch = resultUsers;
            }

            return View();
        }
    }
}