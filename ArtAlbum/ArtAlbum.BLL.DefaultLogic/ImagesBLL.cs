using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.BLL.Abstract;
using ArtAlbum.Entities;
using ArtAlbum.DAL.Abstract;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class ImagesBLL : IImagesBLL
    {
        private IImagesDAL imagesDAL;
        private IUsersImagesDAL relationsDAL;
        private ILikesDAL likesDAL;

        public ImagesBLL(IImagesDAL imagesDAL, IUsersImagesDAL relationsDAL, ILikesDAL likesDAL)
        {
            if (imagesDAL == null || relationsDAL == null || likesDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.imagesDAL = imagesDAL;
            this.relationsDAL = relationsDAL;
            this.likesDAL = likesDAL;
        }

        private bool IsImageCorrect(ImageDTO image) 
        {
            if (image == null)
            {
                throw new ArgumentNullException("image data is null");
            }
            if (image.DateOfCreating > DateTime.Now || image.DateOfCreating.Year < 1960 ) 
            {
                throw new ArgumentNullException("image dateofcreating is null");
            }
            if (string.IsNullOrWhiteSpace(image.Description) && image.Description.Length > 500)
            {
                throw new ArgumentException("incorrect description");
            }
            if (image.Id == null)
            {
                throw new ArgumentException("incorrect Id");
            }
            return true;
        }

        public bool AddImage(ImageDTO image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image data is null");
            }
            else if (!IsImageCorrect(image))
            {
                throw new Exception("IncorrectDataException");
            }

            return imagesDAL.AddImage(image);
        }

        public ImageDTO GetImageById(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            return imagesDAL.GetImageById(imageId);
        }

        public bool RemoveImageById(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            foreach (var userId in relationsDAL.GetUsersIdsByImageId(imageId).ToArray())
            {
                relationsDAL.RemoveRelation(userId, imageId);
            }
            foreach (var like in likesDAL.GetLikesByImageId(imageId))
            {
                likesDAL.RemoveLikeFromImage(like.LikerId, imageId);
            }
            return imagesDAL.RemoveImageById(imageId);
        }

        public bool UpdateImage(ImageDTO image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image data is null");
            }
            else if (!IsImageCorrect(image))
            {
                throw new Exception("IncorrectDataException");
            }
            try
            {
                imagesDAL.GetImageById(image.Id);
            }
            catch
            {
                throw new ArgumentNullException("such image does not exist");
            }
            return imagesDAL.UpdateImage(image);
        }

        public IEnumerable<ImageDTO> GetAllImages()
        {
            return imagesDAL.GetAllImages();
        }
    }
}
