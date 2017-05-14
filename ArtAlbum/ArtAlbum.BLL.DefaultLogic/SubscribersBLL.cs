using ArtAlbum.BLL.Abstract;
using ArtAlbum.DAL.Abstract;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class SubscribersBLL : ISubscribersBLL
    {
        private IUsersDAL usersDAL;
        private ISubscribersDAL subscribersDAL;

        public SubscribersBLL(IUsersDAL usersDAL, ISubscribersDAL subscribersDAL)
        {
            if (usersDAL == null || subscribersDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.usersDAL = usersDAL;
            this.subscribersDAL = subscribersDAL;
        }
        private bool IsRelationExist(Guid userId, Guid subscriberId)
        {
            try
            {
                usersDAL.GetUserById(userId);
                usersDAL.GetUserById(subscriberId);
            }
            catch
            {
                return false;
            }
            return userId != subscriberId;
        }

        public bool AddSubscriberToUser(Guid subscriberId, Guid userId)
        {
            if (userId == null || subscriberId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            if (!IsRelationExist(userId, subscriberId))
            {
                throw new ArgumentException("one of the ids are incorrect, users doesn't exist or equal");
            }
            foreach (var sub in GetSubscribersOfUser(userId))
            {
                if (sub.Id == subscriberId)
                {
                    return false;
                }
            }
            return subscribersDAL.AddSubscriberToUser(subscriberId, userId);
        }

        public IEnumerable<UserDTO> GetSubscribersOfUser(Guid userId)
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
            return usersDAL.GetAllUsers().Join(subscribersDAL.GetSubscribersOfUser(userId),
                   sub => sub.Id, subId => subId, (sub, subId) => new UserDTO
                   { Id = subId, FirstName = sub.FirstName, LastName = sub.LastName, DateOfBirth = sub.DateOfBirth, Email = sub.Email, HashOfPassword = sub.HashOfPassword, Nickname = sub.Nickname });
        }

        public IEnumerable<UserDTO> GetSubscriptionsOfUser(Guid subscriberId)
        {
            if (subscriberId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            try
            {
                usersDAL.GetUserById(subscriberId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("user id is incorrect, user doesn't exist", e);
            }
            return usersDAL.GetAllUsers().Join(subscribersDAL.GetSubscriptionsOfUser(subscriberId),
                   sub => sub.Id, subId => subId, (sub, subId) => new UserDTO
                   { Id = subId, FirstName = sub.FirstName, LastName = sub.LastName, DateOfBirth = sub.DateOfBirth, Email = sub.Email, HashOfPassword = sub.HashOfPassword, Nickname = sub.Nickname });
        }

        public bool RemoveSubscriberFromUser(Guid subscriberId, Guid userId)
        {
            if (userId == null || subscriberId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            if (!IsRelationExist(userId, subscriberId))
            {
                throw new ArgumentException("one of the ids are incorrect, users doesn't exist or equal");
            }
            foreach (var sub in GetSubscribersOfUser(userId))
            {
                if (sub.Id == subscriberId)
                {
                    return subscribersDAL.RemoveSubscriberFromUser(subscriberId, userId);

                }
            }
            return false;
        }
    }
}
