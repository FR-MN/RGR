using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: User
        [Authorize]
        [HttpGet]
        public ActionResult UserProfile()
        {
            var userId = UserVM.GetUserIdByNickname(User.Identity.Name);
            if (userId != Guid.Empty)
            {
                ViewBag.ImagesOfUser = ImageVM.GetImagesByUserId(userId);
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserProfile(HttpPostedFileBase dataImage, ImageVM image)
        {
            if (dataImage != null && (dataImage.ContentType != null))
            {
                byte[] imageData = new byte[dataImage.ContentLength];
                using (BinaryReader reader = new BinaryReader(dataImage.InputStream))
                {
                    for (int i = 0; i < imageData.Length; i++)
                    {
                        imageData[i] = reader.ReadByte();
                    }
                }
                image.Id = Guid.NewGuid();
                image.DateOfCreating = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(image.Description))
                {
                    if (ImageVM.Create(image, imageData, dataImage.ContentType, UserVM.GetUserIdByNickname(User.Identity.Name)))
                    {
                        return RedirectToAction("UserProfile", "Users");
                    }
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult GetUser(string nickname)
        {
            UserVM user = UserVM.GetAllUsers().FirstOrDefault(userData => userData.Nickname == nickname);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (nickname == User.Identity.Name)
            {
                return RedirectToAction("UserProfile");
            }
            ViewBag.ImagesOfUser = ImageVM.GetImagesByUserId(user.Id);
            return View();
        }
    }
}