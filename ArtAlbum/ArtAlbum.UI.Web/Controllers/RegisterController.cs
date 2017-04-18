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
            if (registerData != null)
            {
                if (ModelState.IsValid)
                {
                    UserVM.Create(registerData);
                }
            }
            return View(registerData);
        }
        public JsonResult CheckEmail(string email)
        {
            var res = true;
            foreach (var user in UserVM.GetAllUsers())
            {
                res = user.Email != email;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckNickname(string nickname)
        {
            var res = true;
            foreach (var user in UserVM.GetAllUsers())
            {
                res = user.Nickname != nickname;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckDateOfBirth(DateTime dateOfBirth)
        {
            return Json(dateOfBirth < DateTime.Now, JsonRequestBehavior.AllowGet);
        }
    }
}