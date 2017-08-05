using ArtAlbum.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;
using ArtAlbum.DAL.Abstract;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class LikesBLL : ILikesBLL
    {
        private IUsersDAL usersDAL;
        private ILikesDAL likesDAL;
        private IImagesDAL imagesDAL;

        public LikesBLL(IUsersDAL usersDAL, ILikesDAL likesDAL, IImagesDAL imagesDAL)
        {
            if (usersDAL == null || likesDAL == null || imagesDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.usersDAL = usersDAL;
            this.likesDAL = likesDAL;
            this.imagesDAL = imagesDAL;
        }
        private bool IsDataValid(LikeDTO like, Guid imageId)
        {
            try
            {
                usersDAL.GetUserById(like.LikerId);
                imagesDAL.GetImageById(imageId);
            }
            catch
            {
                return false;
            }
            return like.DateOfLike <= DateTime.Now;
        }
        public bool AddLikeToImage(LikeDTO like, Guid imageId)
        {
            if (like == null || imageId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            if (!IsDataValid(like, imageId))
            {
                throw new ArgumentException("data is incorrect");
            }
            foreach (var imageData in GetLikedImagesByUserId(like.LikerId))
            {
                if (imageData.Id == imageId)
                {
                    return false;
                }
            }
            return likesDAL.AddLikeToImage(like, imageId);
        }

        public int GetCountOfLikes(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ImageDTO> GetLikedImagesByUserId(Guid userId)
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
            return imagesDAL.GetAllImages().Join(likesDAL.GetIdsOfLikedImagesByUserId(userId),
                   image => image.Id, imageId => imageId, (image, imageId) => new ImageDTO
                   { Id = imageId, DateOfCreating = image.DateOfCreating, Description = image.Description });
        }

        public IEnumerable<LikeDTO> GetLikesByImageId(Guid imageId)
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
            return likesDAL.GetLikesByImageId(imageId);
        }

        public bool RemoveLikeFromImage(Guid userId, Guid imageId)
        {
            if (userId == null || imageId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            try
            {
                imagesDAL.GetImageById(imageId);
                usersDAL.GetUserById(userId);
            }
            catch
            {
                throw new ArgumentException("data is incorrect");
            }
            foreach (var imageData in GetLikedImagesByUserId(userId))
            {
                if (imageData.Id == imageId)
                {
                    return likesDAL.RemoveLikeFromImage(userId, imageId);
                }
            }
            return false;
        }

        public bool IsLikedByUser(Guid userId, Guid imageId)
        {
            if (userId == null || imageId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            try
            {
                imagesDAL.GetImageById(imageId);
                usersDAL.GetUserById(userId);
            }
            catch
            {
                throw new ArgumentException("data is incorrect");
            }
            foreach (var userData in GetLikesByImageId(imageId))
            {
                if (userData.LikerId == userId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
