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
    public class UsersBLL : IUserBLL
    {
        private IUsersDAL usersDAL;
        private IUsersImagesDAL relationsDAL;

        public UsersBLL(IUsersDAL usersDAL, IUsersImagesDAL relationsDAL)
        {
            if (usersDAL == null || relationsDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.usersDAL = usersDAL;
            this.relationsDAL = relationsDAL;
        }

        //добавить больше проверок и выровнять чтобы код был не в одну строчку
        private bool IsUserCorrect(UserDTO user)
        {
           
            return !string.IsNullOrWhiteSpace(user.FirstName)
                && !string.IsNullOrWhiteSpace(user.LastName)
                && !string.IsNullOrWhiteSpace(user.Nickname)
                && user.DateOfBirth < DateTime.Now 
                && user.DateOfBirth.Year >= 1900;

        }
        
        public bool AddUser(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("data is null");
            }
            else if (!IsUserCorrect(user))
            {
                throw new Exception("IncorrectData");
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
            foreach (var awardId in relationsDAL.GetImagesIdsByUserId(userId).ToArray())
            {
                relationsDAL.RemoveRelation(userId, awardId);
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
            foreach (var userData in GetAllUsers())
            {
                if (user.Email == userData.Email && user.Nickname == userData.Nickname)
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
    }
}
