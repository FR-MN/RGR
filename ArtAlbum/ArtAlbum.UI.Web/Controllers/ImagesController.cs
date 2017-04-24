using ArtAlbum.UI.Web.Helpers;
using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class ImagesController : Controller
    {
        public ActionResult GetResizedImage(string id, int newWidth, int maxHeight)
        {
            Guid imageId = Guid.Parse(id);
            ImageDataVM imageSrc = ImageDataVM.GetImageById(imageId);
            Image image;
            using (var ms = new MemoryStream(imageSrc.Data))
            {
                image = new Bitmap(ms);
            }
            
            var resizedImage = new Bitmap(image.Resize(newWidth, maxHeight, true));
            using (var streak = new MemoryStream())
            {
                resizedImage.Save(streak, ImageFormat.Png);
                return File(streak.ToArray(), "image/png");
            }
        }

        public ActionResult GetImage(string id)
        {
            Guid imageId = Guid.Parse(id);
            ImageDataVM imageSrc = ImageDataVM.GetImageById(imageId);
            return File(imageSrc.Data, imageSrc.Type);
        }

        [ChildActionOnly]
        public PartialViewResult ShowImages(IEnumerable<ImageVM> images)
        {
            return PartialView("_ImagesPartial", images);
        }
    }
}