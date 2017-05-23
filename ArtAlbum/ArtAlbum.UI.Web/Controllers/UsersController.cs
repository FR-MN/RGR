using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ArtAlbum.UI.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: User
        [Authorize]
        [HttpGet]
        public ActionResult UserProfile()
        {
            var userId = UserVM.GetUserIdByNickname(User.Identity.Name);
           
            if (userId != Guid.Empty)
            {
                UserVM user = UserVM.GetAllUsers().FirstOrDefault(userData => userData.Id == userId);
                ViewBag.ImagesOfUser = ImageVM.GetImagesByUserId(userId);
                ViewBag.Nickname = User.Identity.Name;
                ViewBag.LikedImages = ImageVM.GetImagesLikedByUser(userId);
                ViewBag.Id = userId;
                ViewBag.FirstUserName = user.FirstName;
                ViewBag.LastUserName = user.LastName;
                ViewBag.UserAge = user.Age;
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserProfile(HttpPostedFileBase dataImage, ImageVM image, string tagsData)
        {
            if (dataImage != null && (dataImage.ContentType != null) && !string.IsNullOrWhiteSpace(image.Description))
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
                    image.Id = Guid.NewGuid();
                    if (ImageVM.Create(image, imageData, dataImage.ContentType, UserVM.GetUserIdByNickname(User.Identity.Name)))
                    {
                        if (!string.IsNullOrWhiteSpace(tagsData))
                        {
                            foreach (var tagName in tagsData.Split(' '))
                            {
                                ImageVM.AddTag(tagName, image.Id);
                            }
                        }
                        return RedirectToAction("UserProfile", "Users");
                    }
                }
            }
            
            return View();
        }

        public ActionResult GetAvatar(Guid userId)
        {
            UserAvatarDataVM avatar = UserAvatarDataVM.GetUserAvatarById(userId);
            if (avatar.Data != null && avatar.Type != null)
            {
                return File(avatar.Data, avatar.Type);
            }
            return File("~/Content/img/AvatarDefault.jpg", "image/jpeg");
        }

        [Authorize]
        public JsonResult Subscribe(string userId)
        {

            var currentUserId = UserVM.GetUserIdByNickname(User.Identity.Name);
            var subscribtionUserId = Guid.Parse(userId);

            foreach (var subscription in UserVM.GetSubscribtionsByUserId(currentUserId))
            {
                if (subscription.Id == subscribtionUserId)
                {
                    UserVM.UnsubscribeToUser(subscribtionUserId, currentUserId);
                    return Json("Подписаться",JsonRequestBehavior.AllowGet);
                }
            }
            UserVM.SubscribeToUser(subscribtionUserId, currentUserId);
            return Json("Отписаться", JsonRequestBehavior.AllowGet);
        } 

        [Authorize]
        public ActionResult GetUser(string nickname)
        {
            UserVM user = UserVM.GetAllUsers().FirstOrDefault(userData => userData.Nickname == nickname);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (nickname == User.Identity.Name)
            {
                return RedirectToAction("UserProfile");
            }
            ViewBag.ImagesOfUser = ImageVM.GetImagesByUserId(user.Id);
            ViewBag.UserId = user.Id;
            ViewBag.Nickname = nickname;
            ViewBag.LikedImages = ImageVM.GetImagesLikedByUser(user.Id);
            ViewBag.FirstUserName = user.FirstName;
            ViewBag.LastUserName = user.LastName;
            ViewBag.UserAge = user.Age;
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Settings()
        {
            UserVM user = UserVM.GetUserById(UserVM.GetUserIdByNickname(User.Identity.Name));
            return View(new RegisterVM { FirstName = user.FirstName, LastName = user.LastName, DateOfBirth = user.DateOfBirth, Email = user.Email, Nickname = user.Nickname });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(RegisterVM userData, string password)
        {
            UserVM user = UserVM.GetUserById(UserVM.GetUserIdByNickname(User.Identity.Name));
            userData.Password = password;
            userData.Nickname = user.Nickname;
            userData.Email = user.Email;
            if (userData != null)
            {
                bool isModelValid = ModelState.IsValidField("FirstName") && ModelState.IsValidField("LastName") && ModelState.IsValidField("DateOfBirth") && ModelState.IsValidField("FirstName");
                if (UserVM.IsValid(userData.Nickname, userData.Password) && isModelValid)
                {
                    UserVM.Update(userData);
                    ViewBag.isUpdate = true;
                }
                else
                {
                    ViewBag.isUpdate = false;
                }
            }
            userData.Password = "";
            return View(userData);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(string password)
        {
            if (password != null && UserVM.IsValid(User.Identity.Name, password))
            {
                Guid userId = UserVM.GetUserIdByNickname(User.Identity.Name);
                FormsAuthentication.SignOut();
                UserVM.Remove(userId);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.isValid = false;
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, RegisterVM userData)
        {
            if (currentPassword != null && UserVM.IsValid(User.Identity.Name, currentPassword))
            {
                UserVM user = UserVM.GetUserById(UserVM.GetUserIdByNickname(User.Identity.Name));
                userData.DateOfBirth = user.DateOfBirth;
                userData.Email = user.Email;
                userData.FirstName = user.FirstName;
                userData.LastName = user.LastName;
                userData.Nickname = user.Nickname;
                UserVM.Update(userData);
                ViewBag.isUpdate = true;
                return View();
            }
            ViewBag.isUpdate = false;
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangeAvatar()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(HttpPostedFileBase dataImage, ImageVM image)
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

                Bitmap imageBitmap;
                using (var ms = new MemoryStream(imageData))
                {
                    imageBitmap = new Bitmap(ms);
                }

                if (imageBitmap.Height == imageBitmap.Width && imageBitmap.Height <= 500 && imageBitmap.Width <= 500 && UserAvatarDataVM.AddUserAvatar(imageData, dataImage.ContentType, UserVM.GetUserIdByNickname(User.Identity.Name)))
                {
                    return RedirectToAction("UserProfile", "Users");
                }
            }
            ViewBag.ErrorMessage = "Загружаемая картинка должна быть разрешением не более 500px и иметь соотношение 1:1";
            return View();
        }      

        [Authorize]
        public ActionResult GetSubscribers()
        {
            return View(UserVM.GetAllUsers());
        }

        [Authorize]
        public ActionResult GetSubscribtions()
        {
            return View(UserVM.GetAllUsers());
        }
    }
}