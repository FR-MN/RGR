using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.BLL.Abstract;
using ArtAlbum.Entities;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class UserImageBLL : IUserImageBLL
    {
        private IImagesDAL imagesDAL;
        private IUsersDAL usersDAL;
        private IUsersImagesDAL relationsDAL;


        public UserImageBLL(IUsersDAL usersDAL, IImagesDAL imagesDAL, IUsersImagesDAL relationsDAL)
        {
            if (usersDAL == null || imagesDAL == null || relationsDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.usersDAL = usersDAL;
            this.imagesDAL = imagesDAL;
            this.relationsDAL = relationsDAL;
        }
        private bool IsRelationExist(Guid userId, Guid imageId)
        {
            try
            {
                usersDAL.GetUserById(userId);
                imagesDAL.GetAwardById(imageId);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool AddImageToUser(Guid userId, Guid imageId)
        {
            if (userId == null || imageId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            if (!IsRelationExist(userId, imageId))
            {
                throw new ArgumentException("one of the ids are incorrect, user or award doesn't exist");
            }
            foreach (var user in GetUsersByImage(imageId))
            {
                if (user.Id == userId)
                {
                    return false;
                }
            }
            return relationsDAL.AddRelation(userId, imageId);
        }

      

        public IEnumerable<UserDTO> GetUsersByImage(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveImageFromUser(Guid userId, Guid imageId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ImageDTO> GetImagesByUser(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
