using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class UsersController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult UserProfile()
        {
            ViewBag.ImagesOfUser = ImageVM.GetImagesByUserId(Guid.Parse("03cd9942-aa16-4a9e-95da-a5655fc1f4f2"));
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