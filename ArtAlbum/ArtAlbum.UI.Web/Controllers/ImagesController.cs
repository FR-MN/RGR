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
        
        public ActionResult ShowImagesPage(string typeofrequest, IEnumerable<ImageVM> images)
        {
            var userId = UserVM.GetUserIdByNickname(User.Identity.Name);
            if (typeofrequest == "ShowFavoriteImage")
            {
                ViewBag.ShowedImages =  ImageVM.GetImagesLikedByUser(userId);
                ViewBag.PageName = "Любимые изображения";
                return View();
            }
            if (typeofrequest == "ShowImageOfFollowers")
            {
                List<ImageVM> imagesOfSubscription = new List<ImageVM>();
                foreach (var subscription in UserVM.GetSubscribtionsByUserId(userId))
                {
                    imagesOfSubscription.AddRange(ImageVM.GetImagesByUserId(subscription.Id));
                }
                ViewBag.ShowedImages = imagesOfSubscription;
                ViewBag.PageName = "Активность подписчиков";
                return View();
            }
            else
            {
                userId = Guid.Parse(typeofrequest);
                string username =  UserVM.GetUserById(userId).Nickname;
                ViewBag.ShowedImages = ImageVM.GetImagesLikedByUser(userId);
                ViewBag.PageName = "Любимые изображения пользователя:"+ username;
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
            object[] arr = new object[4];
            arr[0] = false;

            if (!string.IsNullOrWhiteSpace(commentText) && commentText.Length < 500 && imageId != null)
            {
                Guid userId = UserVM.GetUserIdByNickname(User.Identity.Name);
                CommentVM.AddComment(new CommentVM() { Id = Guid.NewGuid(), AuthorId = userId, DateOfCreating = DateTime.Now, Data = commentText }, Guid.Parse(imageId));
                arr[0] = true;
                arr[1] = userId.ToString();
                arr[2] = User.Identity.Name;
                arr[3] = DateTime.Now.ToString();
            }

            return Json(arr, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //public JsonResult RemoveComment(string commentId, string imageId)
        //{
        //    object[] arr = new object[4];
        //    arr[0] = false;

        //    if (commentId != null && imageId != null)
        //    {
        //        Guid userId = UserVM.GetUserIdByNickname(User.Identity.Name);
        //        CommentVM.AddComment(new CommentVM() { Id = Guid.NewGuid(), AuthorId = userId, DateOfCreating = DateTime.Now, Data = commentText }, Guid.Parse(imageId));
        //        arr[0] = true;
        //        arr[1] = userId.ToString();
        //        arr[2] = User.Identity.Name;
        //        arr[3] = DateTime.Now.ToString();
        //    }

        //    return Json(arr, JsonRequestBehavior.AllowGet);
        //}

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