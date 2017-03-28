using ArtAlbum.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.DataBase
{
    public class UsersImagesDAL : IUsersImagesDAL
    {
        public bool AddRelation(Guid userId, Guid awardId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> GetImagesIdsByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> GetUsersIdsByImageId(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRelation(Guid userId, Guid awardId)
        {
            throw new NotImplementedException();
        }
    }
}
