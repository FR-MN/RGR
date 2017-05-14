using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.Abstract
{
    public interface ILikesDAL
    {
        IEnumerable<LikeDTO> GetLikesByImageId(Guid imageId);
        IEnumerable<Guid> GetIdsOfLikedImagesByUserId(Guid userId);
        bool AddLikeToImage(LikeDTO like, Guid imageId);
        bool RemoveLikeFromImage(Guid userId, Guid imageId);
    }
}
