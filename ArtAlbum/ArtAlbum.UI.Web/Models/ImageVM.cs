using ArtAlbum.BLL.Abstract;
using ArtAlbum.DI.Provaiders;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class ImageVM
    {
        private static IImagesBLL imagesLogic = Provaider.ImagesBLL;
        private static IUsersImagesBLL relationsLogic = Provaider.RelationsBLL;

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
        public byte[] Data { get; set; }
        public string Type { get; set; }

        internal static bool Create(ImageVM image)
        {
            return imagesLogic.AddImage(image);
        }

        public override string ToString()
        {
            return string.Format("Description: {0}, date of creating: {1}", Description, DateOfCreating);
        }

        public static explicit operator ImageVM(ImageDTO data)
        {
            return new ImageVM() { Id = data.Id, Description = data.Description, DateOfCreating = data.DateOfCreating, Data = data.Data, Type = data.Type };
        }
        public static implicit operator ImageDTO(ImageVM data)
        {
            return new ImageDTO() { Id = data.Id, Description = data.Description, DateOfCreating = data.DateOfCreating, Data = data.Data, Type = data.Type };
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
    }
}