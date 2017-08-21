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

        [Authorize]
        public JsonResult UpdateLike(string isitLikeMe , string imageId)
        {
            var userId = UserVM.GetUserIdByNickname(User.Identity.Name);
            Guid imageIdData = Guid.Empty;
            bool isImageIdValid = Guid.TryParse(imageId, out imageIdData);
            if (isImageIdValid)
            {
                if (ImageVM.IsImageLikedByUser(userId, imageIdData))
                {
                    ImageVM.RemoveLikeFromImage(userId, imageIdData);
                    return Json("Нравится", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ImageVM.AddLikeToImage(userId, imageIdData);
                    return Json("Не нравится", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Недоступно",JsonRequestBehavior.AllowGet);

        }

        [ChildActionOnly]
        public PartialViewResult ShowImages(IEnumerable<ImageVM> images)
        {
            return PartialView("_ImagesPartial", images);
        }

        [ChildActionOnly]
        public PartialViewResult ShowSmallImages(IEnumerable<ImageVM> images)
        {
            return PartialView("_SmallImagesPartial", images);
        }

        [Authorize]
        public ActionResult ListImages(string nameOfList, string userId = "")
        {
            ViewBag.NameOfImages = nameOfList;
            if (nameOfList == "Недавние работы")
            {
                return View(ImageVM.GetRecentImages(200));
            }
            else if (nameOfList == "Активность ваших подписок")
            {
                List<ImageVM> imagesOfSubscription = new List<ImageVM>();
                foreach (var subscription in UserVM.GetSubscribtionsByUserId(UserVM.GetUserIdByNickname(User.Identity.Name)))
                {
                    imagesOfSubscription.AddRange(ImageVM.GetImagesByUserId(subscription.Id));
                }
                return View(imagesOfSubscription.Take(200));
            }
            else if (nameOfList == "Любимые изображения")
            {
                return View(ImageVM.GetImagesLikedByUser(UserVM.GetUserIdByNickname(User.Identity.Name)));
            }
            else if (nameOfList == "Мои изображения")
            {
                return View(ImageVM.GetImagesByUserId(UserVM.GetUserIdByNickname(User.Identity.Name)));
            }
            else if (nameOfList == "Любимые изображения пользователя")
            {
                ViewBag.NameOfImages = nameOfList + " " + UserVM.GetUserById(Guid.Parse(userId)).Nickname;
                return View(ImageVM.GetImagesByUserId(Guid.Parse(userId)));
            }
            else if (nameOfList == "Изображения пользователя")
            {
                ViewBag.NameOfImages = nameOfList + " " + UserVM.GetUserById(Guid.Parse(userId)).Nickname;
                return View(ImageVM.GetImagesByUserId(Guid.Parse(userId)));
            }
            else if (nameOfList.Length == 2)
            {
                string countryName = MapHelper.GetCountryNameByCode(nameOfList);
                if (!string.IsNullOrWhiteSpace(countryName))
                {
                    ViewBag.NameOfImages = "Недавние фото из страны " + countryName;
                    return View(ImageVM.GetImagesByCountryCode(nameOfList));
                }
            }
            return View();
        }
        
        [HttpGet]
        public PartialViewResult Comment(Guid imageId)
        {
            ViewBag.ImageId = imageId;
            return PartialView("_CommentsPartial", CommentVM.GetCommentsByImageId(imageId).OrderBy(comment => comment.DateOfCreating));
        }

        [Authorize]
        public JsonResult AddComment(string commentText, string imageId)
        {
            object[] arr = new object[5];
            arr[0] = false;

            if (!string.IsNullOrWhiteSpace(commentText) && commentText.Length < 500 && imageId != null)
            {
                Guid commentId = Guid.NewGuid();
                Guid userId = UserVM.GetUserIdByNickname(User.Identity.Name);
                CommentVM.AddComment(new CommentVM() { Id = commentId, AuthorId = userId, DateOfCreating = DateTime.Now, Data = commentText }, Guid.Parse(imageId));
                arr[0] = true;
                arr[1] = userId.ToString();
                arr[2] = User.Identity.Name;
                arr[3] = DateTime.Now.ToString();
                arr[4] = commentId;
            }

            return Json(arr, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult DeleteComment(string commentId)
        {
            Guid commentGuidId = Guid.Parse(commentId);
            if (commentId != null && CommentVM.IsCommentExist(commentGuidId))
            {
                return Json(CommentVM.RemoveComment(commentGuidId), JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Delete(Guid imageId)
        {
            if (ImageVM.RemoveImage(imageId))
            {
                return RedirectToAction("UserProfile", "Users");
            }
            return View();
        }
    }
}