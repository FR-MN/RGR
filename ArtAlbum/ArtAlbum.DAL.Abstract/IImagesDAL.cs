using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.Abstract
{
    public interface IImagesDAL
    {
        bool AddImage(ImageDTO image);
        UserDTO GetImageById(Guid imageId);
        bool RemoveImageById(Guid imageId);
        bool UpdateImage(ImageDTO image);
        IEnumerable<ImageDTO> GetAllImages();
    }
}
