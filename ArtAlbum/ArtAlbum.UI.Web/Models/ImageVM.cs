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
        private static ITagsBLL tagsLogic = Provider.TagsBLL;

        private string description;

        public Guid Id { get; set; }
        public DateTime DateOfCreating { get; set; }
        public string Country { get; set; }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }

        internal static bool Create(ImageVM image, byte[] data, string type, Guid authorId, string countryCode)
        {

            return imagesLogic.AddImage(new ImageDTO() { Id = image.Id, Description = image.Description, DateOfCreating = image.DateOfCreating, Type = type, Data = data, Country = countryCode }) && relationsLogic.AddImageToUser(authorId, image.Id);
        }

        public override string ToString()
        {
            return string.Format("Description: {0}, date of creating: {1}", Description, DateOfCreating);
        }

        public static explicit operator ImageVM(ImageDTO data)
        {
            return new ImageVM() { Id = data.Id, Description = data.Description, DateOfCreating = data.DateOfCreating, Country = data.Country };
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

        public static IEnumerable<ImageVM> GetRecentImages(int count)
        {
            if (count > 0)
            {
                List<ImageVM> list = new List<ImageVM>();
                foreach (var image in imagesLogic.GetAllImages().OrderByDescending(image => image.DateOfCreating).Take(count))
                {
                    list.Add((ImageVM)image);
                }
                return list;
            }
            return null;
        }

        public static int GetCountOfImagesByCountryCode(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return 0;
            }
            return imagesLogic.GetCountOfImagesByCountry(countryCode);
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

        public static bool RemoveImage(Guid imageId)
        {
            return imagesLogic.RemoveImageById(imageId);
        }

        public static bool AddTag(string tagName, Guid imageId)
        {
            if (!string.IsNullOrWhiteSpace(tagName) && tagName.Length <= 50)
            {
                return tagsLogic.AddTagToImage(new TagDTO() { Id = Guid.NewGuid(), Name = tagName.ToLower() }, imageId);
            }
            return false;
        }

        public static IEnumerable<string> GetTagsByImage(Guid imageId)
        {
            List<string> list = new List<string>();
            foreach (var tag in tagsLogic.GetTagsByImageId(imageId))
            {
                list.Add(tag.Name);
            }
            return list;
        }

        public static int CountOfImagesWithTag(string tagName)
        {
            return tagsLogic.GetImagesByTagId(tagsLogic.GetAllTags().Where(tag => tag.Name == tagName).First().Id).Count();
        }

        public static IEnumerable<string> GetAllTags()
        {
            List<string> list = new List<string>();
            foreach (var tag in tagsLogic.GetAllTags())
            {
                list.Add(tag.Name);
            }
            return list;
        }

        public static IEnumerable<ImageVM> GetImagesByCountryCode(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return null;
            }
            var tmp = GetAllImages().Where(image => image.Country == countryCode).OrderByDescending(image => image.DateOfCreating).Take(32);
            return GetAllImages().Where(image => image.Country == countryCode).OrderByDescending(image => image.DateOfCreating).Take(32);
        }

        //public static int GetCountOfImagesByCountryCode(string countryCode)
        //{
        //    if (string.IsNullOrWhiteSpace(countryCode))
        //    {
        //        return 0;
        //    }
        //    return GetAllImages().Where(image => image.Country == countryCode).Count();
        //}
    }
}