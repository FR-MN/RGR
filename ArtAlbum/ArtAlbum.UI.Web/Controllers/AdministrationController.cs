using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class AdministrationController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult UserAdministration()
        {
            IEnumerable<UserVM> users = UserVM.GetAllUsers().Where(user => user.Nickname != User.Identity.Name);
            return View(users);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ExpertAdministration()
        {
           
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(Guid userId)
        {
            UserVM.Remove(userId);
            return RedirectToAction("UserAdministration", "Users");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddAdministrator(Guid userId)
        {
            RoleVM.AddRoleToUser(UserVM.GetUserById(userId).Nickname, "Admin");
            return RedirectToAction("UserAdministration", "Administration");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveAdministrator(Guid userId)
        {
            RoleVM.RemoveRoleFromUser(UserVM.GetUserById(userId).Nickname, "Admin");
            return RedirectToAction("UserAdministration", "Administration");
        }
    }
}