using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class MapsController : Controller
    {
        // GET: Maps
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CountOfImages(string countryCode)
        {
            return Json(new { count = Models.ImageVM.GetCountOfImagesByCountryCode(countryCode).ToString(), name = Helpers.MapHelper.GetCountryNameByCode(countryCode) }, JsonRequestBehavior.AllowGet);
        }
    }
}