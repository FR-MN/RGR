using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.BLL.Abstract
{
    public interface ILikesBLL
    {
        int GetCountOfLikes(Guid imageId);
        IEnumerable<LikeDTO> GetLikesByImageId(Guid imageId);
        IEnumerable<ImageDTO> GetLikedImagesByUserId(Guid userId);
        bool AddLikeToImage(LikeDTO like, Guid imageId);
        bool RemoveLikeFromImage(Guid userId, Guid imageId);
    }
}
