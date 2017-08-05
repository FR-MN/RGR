using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.BLL.Abstract;
using ArtAlbum.Entities;
using ArtAlbum.DAL.Abstract;
using System.Text.RegularExpressions;
using ArtAlbum.BLL.DefaultLogic.Exceptions;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class UsersBLL : IUsersBLL
    {
        private IUsersDAL usersDAL;
        private IUsersImagesDAL relationsDAL;
        private IRolesDAL rolesDAL;
        private ISubscribersDAL subscribersDAL;
        private ILikesDAL likesDAL;
        private IUsersAvatarsDAL avatarsDAL;
        private ICommentsDAL commentsDAL;
        private IImagesDAL imagesDAL;
        private ITagsDAL tagsDAL;
        private static Regex regexEmail;
        private static Regex regexNickname;
        private static Regex regexName;

        static UsersBLL()
        {
            regexEmail = new Regex(@"^[-\w.]+@([A-z0-9][-A-z0-9]*\.)+[A-z]{2,4}$");
            regexNickname = new Regex(@"^[a-zA-Z][a-zA-Z0-9-_\.]{0,20}$");
            regexName = new Regex(@"^[а-яА-ЯёЁa-zA-Z]+$");
        }
        public UsersBLL(IUsersDAL usersDAL, IUsersImagesDAL relationsDAL, IRolesDAL rolesDAL, ISubscribersDAL subscribersDAL, ILikesDAL likesDAL, IUsersAvatarsDAL avatarsDAL, ICommentsDAL commentsDAL, IImagesDAL imagesDAL, ITagsDAL tagsDAL)
        {
            if (usersDAL == null || relationsDAL == null || subscribersDAL == null || rolesDAL == null || likesDAL == null || avatarsDAL == null || commentsDAL == null || imagesDAL == null || tagsDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.usersDAL = usersDAL;
            this.relationsDAL = relationsDAL;
            this.rolesDAL = rolesDAL;
            this.subscribersDAL = subscribersDAL;
            this.likesDAL = likesDAL;
            this.avatarsDAL = avatarsDAL;
            this.commentsDAL = commentsDAL;
            this.imagesDAL = imagesDAL;
            this.tagsDAL = tagsDAL;
        }

        private bool IsUserCorrect(UserDTO user)
        {

            return !string.IsNullOrWhiteSpace(user.FirstName)
                && !string.IsNullOrWhiteSpace(user.LastName)
                && !string.IsNullOrWhiteSpace(user.Nickname)
                && !string.IsNullOrWhiteSpace(user.Email)
                && user.DateOfBirth < DateTime.Now
                && user.DateOfBirth.Year >= 1900
                && user.FirstName.Length < 50
                && user.LastName.Length < 50
                && user.Nickname.Length < 50
                && user.Email.Length < 100
                && regexEmail.IsMatch(user.Email)
                && regexNickname.IsMatch(user.Nickname)
                && regexName.IsMatch(user.FirstName)
                && regexName.IsMatch(user.LastName);
        }
        
        public bool AddUser(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user data is null");
            }
            else if (!IsUserCorrect(user))
            {
                throw new IncorrectDataException();
            }
            foreach (var userData in GetAllUsers())
            {
                if (user.Email == userData.Email && user.Nickname == userData.Nickname)
                {
                    return false;
                }
            }
            return usersDAL.AddUser(user);
        }

        public UserDTO GetUserById(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            return usersDAL.GetUserById(userId);
        }

        public bool RemoveUserById(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            foreach (var imageId in relationsDAL.GetImagesIdsByUserId(userId).ToArray())
            {
                relationsDAL.RemoveRelation(userId, imageId);
                foreach (var commentId in commentsDAL.GetCommentsByImageId(imageId))
                {
                    commentsDAL.RemoveCommentFromImage(commentId, imageId);
                    commentsDAL.RemoveCommment(commentId);
                }
                foreach (var like in likesDAL.GetLikesByImageId(imageId))
                {
                    likesDAL.RemoveLikeFromImage(like.LikerId, imageId);
                }
                foreach (var tagId in tagsDAL.GetTagsByImageId(imageId))
                {
                    tagsDAL.RemoveTagFromImage(tagId, imageId);
                    if (tagsDAL.GetImagesByTagId(tagId) == null && tagsDAL.GetImagesByTagId(tagId).Count() == 0)
                    {
                        tagsDAL.RemoveTag(tagId);
                    }
                }
                imagesDAL.RemoveImageById(imageId);
            }
            foreach (var roleId in rolesDAL.GetRolesIdsByUserId(userId).ToArray())
            {
                rolesDAL.RemoveRoleFromUser(userId, roleId);
            }
            foreach (var subId in subscribersDAL.GetSubscribersOfUser(userId))
            {
                subscribersDAL.RemoveSubscriberFromUser(subId, userId);
            }
            foreach (var subscriptionId in subscribersDAL.GetSubscriptionsOfUser(userId))
            {
                subscribersDAL.RemoveSubscriberFromUser(userId, subscriptionId);
            }
            foreach (var imageId in likesDAL.GetIdsOfLikedImagesByUserId(userId))
            {
                likesDAL.RemoveLikeFromImage(userId, imageId);
            }
            foreach (var commentId in commentsDAL.GetAllComments().Where(comment => comment.AuthorId == userId).Select(comment => comment.Id))
            {
                commentsDAL.RemoveCommment(commentId);
            }
            return usersDAL.RemoveUserById(userId);
        }

        public bool UpdateUser(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user data is null");
            }
            else if (!IsUserCorrect(user))
            {
                throw new Exception("IncorrectData");
            }
            try
            {
                usersDAL.GetUserById(user.Id);
            }
            catch
            {
                throw new ArgumentNullException("such user does not exist");
            }
            foreach (var userData in GetAllUsers())
            {
                if (user.Email == userData.Email && user.Nickname == userData.Nickname && user.Id != userData.Id)
                {
                    return false;
                }
            }
            return usersDAL.UpdateUser(user);
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return usersDAL.GetAllUsers();
        }

        public bool AddUserAvatar(UserAvatarDTO userAvatar)
        {
            if (userAvatar == null)
            {
                throw new ArgumentNullException("user data is null");
            }
            try
            {
                usersDAL.GetUserById(userAvatar.UserId);
            }
            catch
            {
                throw new ArgumentNullException("such user does not exist");
            }
            return avatarsDAL.AddUserAvatar(userAvatar);
        }

        public UserAvatarDTO GetUserAvatarById(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            return avatarsDAL.GetUserAvatarById(userId);
        }

        public bool RemoveUserAvatarById(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("user data is null");
            }
            return avatarsDAL.RemoveUserAvatarById(userId);
        }
    }
}
