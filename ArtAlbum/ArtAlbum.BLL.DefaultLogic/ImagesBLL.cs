using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.BLL.Abstract;
using ArtAlbum.Entities;


namespace ArtAlbum.BLL.DefaultLogic
{
    public class ImagesBLL :IImageBLL
    {
        private IImagesDAL imagesDAL;
        private IUserImageDAL relationsDAL;

        public ImagesBLL(IImagesDAL awardsDAL, IUsersImagesDAL relationsDAL)
        {
            if (awardsDAL == null || relationsDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.imagesDAL = awardsDAL;
            this.relationsDAL = relationsDAL;
        }

        private bool IsImageCorrect(ImageDTO award) // fix;
        {
            //Какие проверки надо накладывать на изображение
            return true;
        }

        public bool AddImage(ImageDTO award)
        {
            if (award == null)
            {
                throw new ArgumentNullException("award data is null");
            }
            else if (!IsImageCorrect(award))
            {
                throw new IncorrectDataException();
            }

            return imagesDAL.AddAward(award);
        }

        public ImageDTO GetImageById(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("award id is null");
            }
            return imagesDAL.GetAwardById(imageId);
        }

        public bool RemoveImageById(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("award id is null");
            }
            foreach (var userId in relationsDAL.GetUsersIdsByAwardId(imageId).ToArray())
            {
                relationsDAL.RemoveRelation(userId, imageId);
            }
            return imagesDAL.RemoveAwardById(imageId);
        }

        public bool UpdateImage(ImageDTO award)
        {
            if (award == null)
            {
                throw new ArgumentNullException("award data is null");
            }
            else if (!IsImageCorrect(award))
            {
                throw new IncorrectDataException();
            }

            return imagesDAL.UpdateAward(award);
        }

        public IEnumerable<ImageDTO> GetAllImages()
        {
            return imagesDAL.GetAllImages();
        }
    }
}
