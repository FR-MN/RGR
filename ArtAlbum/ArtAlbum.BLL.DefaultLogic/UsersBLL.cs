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
        private static Regex regexEmail;
        private static Regex regexNickname;
        private static Regex regexName;

        static UsersBLL()
        {
            regexEmail = new Regex(@"^[-\w.]+@([A-z0-9][-A-z0-9]*\.)+[A-z]{2,4}$");
            regexNickname = new Regex(@"^[a-zA-Z][a-zA-Z0-9-_\.]{0,20}$");
            regexName = new Regex(@"^[а-яА-ЯёЁa-zA-Z]+$");
        }
        public UsersBLL(IUsersDAL usersDAL, IUsersImagesDAL relationsDAL)
        {
            if (usersDAL == null || relationsDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.usersDAL = usersDAL;
            this.relationsDAL = relationsDAL;
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
    }
}
