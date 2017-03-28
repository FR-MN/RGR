using ArtAlbum.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;

namespace ArtAlbum.DAL.DataBase
{
    public class ImagesDAL : IImagesDAL
    {
        public bool AddImage(ImageDTO image)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ImageDTO> GetAllImages()
        {
            throw new NotImplementedException();
        }

        public UserDTO GetImageById(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveImageById(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateImage(ImageDTO image)
        {
            throw new NotImplementedException();
        }
    }
}
