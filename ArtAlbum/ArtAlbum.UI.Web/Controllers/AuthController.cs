using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ArtAlbum.UI.Web.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(AuthVM data)
        {
            if (ModelState.IsValid)
            {
                if (UserVM.IsValid(data.Nickname, data.Password))
                {
                    FormsAuthentication.SetAuthCookie(data.Nickname, false);
                    var url = Request.Params.Get("ReturnUrl");
                    {
                        if (url != null)
                        {
                            return Redirect(url);
                        }
                    }
                    return RedirectToAction("UserProfile", "Users");
                }
            }
            return View(data);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Home");
        }
    }
}