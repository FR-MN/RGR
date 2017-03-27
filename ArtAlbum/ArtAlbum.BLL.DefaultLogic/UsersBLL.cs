using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.BLL.Abstract;
using ArtAlbum.Entities;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class UsersBLL:IUserBLL
    {
        private IUserDAL usersDAL;
        private IUserImageDAL relationsDAL;

        public UsersBLL(IUsersDAL usersDAL, IUsersImagesDAL relationsDAL)//не уверен на счет названия
        {
            if (usersDAL == null || relationsDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.usersDAL = usersDAL;
            this.relationsDAL = relationsDAL;
        }

        private bool IsUserCorrect(UserDTO user)// четкая форма записи,знаю
           
        {
            return !string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.LastName) && user.DateOfBirth < DateTime.Now && user.DateOfBirth.Year >= 1900 ;
        }

        public bool AddUser(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("data is null");
            }
            else if (!IsUserCorrect(user))
            {
                throw new IncorrectDataException();
            }
            foreach (var userData in GetAllUsers())
            {
                if (user.FirstName == userData.FirstName && user.LastName == userData.LastName && user.DateOfBirth == userData.DateOfBirth)// забыл как работает такое и И что будет если фамилии одинаковые...
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
            foreach (var awardId in relationsDAL.GetAwardsIdsByUserId(userId).ToArray())
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
                throw new IncorrectDataException();
            }
            foreach (var userData in GetAllUsers())
            {
                if (user.FirstName == userData.FirstName && user.LastName == userData.LastName && user.DateOfBirth == userData.DateOfBirth)// забыл как работает такое и И что будет если фамилии одинаковые...
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
