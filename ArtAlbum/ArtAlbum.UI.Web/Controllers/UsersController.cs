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
                    if (ImageVM.Create(image, imageData, dataImage.ContentType))
                    {
                        return RedirectToAction("UserProfile", "User");
                    }
                }
            }
            return View();
        }
    }
}