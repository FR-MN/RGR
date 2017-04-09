using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class ArtAlbumController : Controller
    {
        // GET: ArtAlbum
        public ActionResult Index()
        {
            return View(ImageVM.GetAllImages());
        }

        [HttpGet]
        public ActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase dataImage, ImageVM image)
        {
            if (dataImage != null && (dataImage.ContentType != null))
            {
                image.Type = dataImage.ContentType;
                image.Data = new byte[dataImage.ContentLength];
                using(BinaryReader reader = new BinaryReader(dataImage.InputStream))
                {
                    for (int i = 0; i < image.Data.Length; i++)
                    {
                        image.Data[i] = reader.ReadByte();
                    }
                }
                image.Id = Guid.NewGuid();
                image.DateOfCreating = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(image.Description))
                {
                    if (ImageVM.Create(image))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        public ActionResult GetImage(string id)
        {
            Guid imageId = Guid.Parse(id);
            return File(ImageVM.GetImageById(imageId).Data, ImageVM.GetImageById(imageId).Type);
        }
    }
}