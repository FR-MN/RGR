using ArtAlbum.BLL.Abstract;
using ArtAlbum.DI.Providers;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class ImageVM
    {
        private static IImagesBLL imagesLogic = Provider.ImagesBLL;
        private static IUsersImagesBLL relationsLogic = Provider.RelationsBLL;
        private static ILikesBLL likesLogic = Provider.LikesBLL;

        private string description;

        public Guid Id { get; set; }
        public DateTime DateOfCreating { get; set; }
        public string Description
        {
            get { return description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) && value.Length > 500)
                {
                    throw new ArgumentException("incorrect description");
                }
                description = value;
            }
        }

        internal static bool Create(ImageVM image, byte[] data, string type, Guid authorId)
        {
            Guid imageId = Guid.NewGuid();
            return imagesLogic.AddImage(new ImageDTO() { Id = imageId, Description = image.Description, DateOfCreating = image.DateOfCreating, Type = type, Data = data }) && relationsLogic.AddImageToUser(authorId, imageId);
        }

        public override string ToString()
        {
            return string.Format("Description: {0}, date of creating: {1}", Description, DateOfCreating);
        }

        public static explicit operator ImageVM(ImageDTO data)
        {
            return new ImageVM() { Id = data.Id, Description = data.Description, DateOfCreating = data.DateOfCreating };
        }

        public static IEnumerable<ImageVM> GetAllImages()
        {
            List<ImageVM> list = new List<ImageVM>();
            foreach (var image in imagesLogic.GetAllImages())
            {
                list.Add((ImageVM)image);
            }
            return list;
        }

        public static ImageVM GetImageById(Guid imageId)
        {
            return (ImageVM)imagesLogic.GetImageById(imageId);
        }

        public static IEnumerable<ImageVM> GetImagesByUserId(Guid userId)
        {
            List<ImageVM> list = new List<ImageVM>();
            foreach (var image in relationsLogic.GetImagesByUser(userId))
            {
                list.Add((ImageVM)image);
            }
            return list;
        }

        public static bool AddLikeToImage(Guid userId, Guid imageId)
        {
            LikeDTO like = new LikeDTO() { DateOfLike = DateTime.Now, LikerId = userId };
            return likesLogic.AddLikeToImage(like, imageId);
        }

        public static bool RemoveLikeFromImage(Guid userId, Guid imageId)
        {
            return likesLogic.RemoveLikeFromImage(userId, imageId);
        }

        public static bool IsImageLikedByUser(Guid userId, Guid imageId)
        {
            return likesLogic.IsLikedByUser(userId, imageId);
        }

        internal static IEnumerable<ImageVM> GetImagesLikedByUser(Guid userId)
        {
            List<ImageVM> list = new List<ImageVM>();
            foreach (var image in likesLogic.GetLikedImagesByUserId(userId))
            {
                list.Add((ImageVM)image);
            }
            return list;
        }
        public static IEnumerable<UserVM> GetUserByImageId(Guid imageId)
        {

            List<UserVM> list = new List<UserVM>();
            foreach (var image in relationsLogic.GetUsersByImage(imageId))
            {
                list.Add((UserVM)image);
            }
            return list;
            
        }
    }
}