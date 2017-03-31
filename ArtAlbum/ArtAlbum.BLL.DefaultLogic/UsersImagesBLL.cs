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
    public class UsersImagesBLL : IUsersImagesBLL
    {
        private IImagesDAL imagesDAL;
        private IUsersDAL usersDAL;
        private IUsersImagesDAL relationsDAL;

        public UsersImagesBLL(IUsersDAL usersDAL, IImagesDAL imagesDAL, IUsersImagesDAL relationsDAL)
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
                imagesDAL.GetImageById(imageId);
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
                throw new ArgumentException("one of the ids are incorrect, user or image doesn't exist");
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
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            try
            {
                imagesDAL.GetImageById(imageId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("image id is incorrect, image doesn't exist", e);
            }
            return usersDAL.GetAllUsers().Join(relationsDAL.GetUsersIdsByImageId(imageId),
                user => user.Id, userId => userId, (user, userId) => new UserDTO
                { Id = userId, FirstName = user.FirstName, DateOfBirth = user.DateOfBirth ,Email = user.Email,HashOfPassword = user.HashOfPassword,LastName = user.LastName,Nickname=user.LastName });

        }
        public IEnumerable<ImageDTO> GetImagesByUser(Guid userId)
        {

            if (userId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            try
            {
                usersDAL.GetUserById(userId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("user id is incorrect, user doesn't exist", e);
            }
            return imagesDAL.GetAllImages().Join(relationsDAL.GetImagesIdsByUserId(userId),
                   image => image.Id, imageId => imageId, (image, imageId) => new ImageDTO
                   { Id = imageId,DateOfCreating = image.DateOfCreating,Description = image.Description});

        }
        public bool RemoveImageFromUser(Guid userId, Guid imageId)
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
                    return relationsDAL.RemoveRelation(userId, imageId);

                }
            }
            return false;
        }

       
    }
}
