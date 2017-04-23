using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class ImagesController : Controller
    {
        public ActionResult GetImage(string id)
        {
            Guid imageId = Guid.Parse(id);
            return File(ImageVM.GetImageById(imageId).Data, ImageVM.GetImageById(imageId).Type);
        }

        public PartialViewResult ShowImages(IEnumerable<ImageVM> images)
        {
            return PartialView("_ImagesPartial", images);
        }
    }
}