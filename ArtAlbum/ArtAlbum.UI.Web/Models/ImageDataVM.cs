using ArtAlbum.BLL.Abstract;
using ArtAlbum.DI.Provaiders;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class ImageDataVM
    {
        private static IImagesBLL imagesLogic = Provaider.ImagesBLL;

        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }

        public static explicit operator ImageDataVM(ImageDTO data)
        {
            return new ImageDataVM() { Id = data.Id, Data = data.Data, Type = data.Type };
        }

        public static ImageDataVM GetImageById(Guid imageId)
        {
            return (ImageDataVM)imagesLogic.GetImageById(imageId);
        }
    }
}