using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;

namespace ArtAlbum.BLL.Abstract
{
    public interface IImageBLL
    {
        bool AddImage(ImageDTO image);
        ImageDTO GetImageById(Guid imageId);
        bool RemoveImageById(Guid imageId);
        bool UpdateImage(ImageDTO user);
        IEnumerable<ImageDTO> GetAllImages();
    }
}
