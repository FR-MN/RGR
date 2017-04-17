using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Register()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterVM registerData)
        {
            if(registerData!=null)
            {
                if(ModelState.IsValid)
                {
                    //todo add user
                }
            }
            return View(registerData);
        }
        public JsonResult CheckEmail(string Email)
        {
            var res = true;
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckLogin(string Login)
        {
            var res = true;
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}