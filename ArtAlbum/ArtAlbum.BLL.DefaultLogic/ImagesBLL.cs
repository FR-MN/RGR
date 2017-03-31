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

        public ImagesBLL(IImagesDAL imagesDAL, IUsersImagesDAL relationsDAL)
        {
            if (imagesDAL == null || relationsDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.imagesDAL = imagesDAL;
            this.relationsDAL = relationsDAL;
        }
        // крутая проверка на нал
        private bool IsImageCorrect(ImageDTO image) // fix;
        {
            if (image == null)
            {
                throw new ArgumentNullException("image data is null");
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

            return imagesDAL.UpdateImage(image);
        }

        public IEnumerable<ImageDTO> GetAllImages()
        {
            return imagesDAL.GetAllImages();
        }
    }
}
