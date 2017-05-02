using ArtAlbum.BLL.Abstract;
using ArtAlbum.BLL.DefaultLogic;
using ArtAlbum.DI.Providers;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class UserVM
    {
        private static IUsersBLL usersLogic = Provider.UsersBLL;
        private static IUsersImagesBLL relationsLogic = Provider.RelationsBLL;

        private string firstName;
        private string lastName;
        private string nickname;
        private string email;
        private DateTime dateOfBirth;
        private static Regex regexEmail;
        private static Regex regexNickname;
        private static Regex regexName;
        private static SHA256 hashFunction = SHA256.Create();

        static UserVM()
        {
            regexEmail = new Regex(@"^[-\w.]+@([A-z0-9][-A-z0-9]+\.)+[A-z]{2,4}$");
            regexNickname = new Regex(@"^[a-zA-Z0-9][a-zA-Z0-9_-]+[a-zA-Z0-9]$");
            regexName = new Regex(@"^[a-zA-Zа-яА-Я]+((-[a-zA-Zа-яА-Я]+)+)|([a-zA-Zа-яА-Я]+)$");
        }

        public Guid Id { get; set; }
        public byte[] HashOfPassword { get; set; }
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) && value.Length > 50 && !regexName.IsMatch(value))
                {
                    throw new ArgumentException("incorrect first name");
                }
                firstName = value;
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) && value.Length > 50 && !regexName.IsMatch(value))
                {
                    throw new ArgumentException("incorrect second name");
                }
                lastName = value;
            }
        }
        public string Nickname
        {
            get { return nickname; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) && value.Length > 50 && !regexNickname.IsMatch(value))
                {
                    throw new ArgumentException("incorrect nickname");
                }
                nickname = value;
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) && !regexEmail.IsMatch(value))
                {
                    throw new ArgumentException("incorrect email");
                }
                email = value;
            }
        }
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                if (value >= DateTime.Now || value.Year < 1900 || value == null)
                {
                    throw new ArgumentException("incorrect date of birth");
                }
                dateOfBirth = value;
            }
        }
        public int Age
        {
            get
            {
                DateTime dateTimeNow = DateTime.Now;
                int age = dateTimeNow.Year - dateOfBirth.Year;
                if (dateOfBirth > dateTimeNow.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }

        internal static bool Add(RegisterVM user)
        {
            var hash = Encoding.UTF8.GetBytes(user.Password);
            hashFunction.ComputeHash(hash);
            return usersLogic.AddUser(new UserDTO { Id = Guid.NewGuid(), FirstName = user.FirstName, LastName = user.LastName, DateOfBirth = user.DateOfBirth, Email = user.Email, HashOfPassword = hash, Nickname = user.Nickname }) && RoleVM.AddRoleToUser(user.Nickname, "User");
        }

        public override string ToString()
        {
            return string.Format("FirstName: {0} , LastName: {1}, date of birth: {2}, age: {3}, email: {4}, nickname: {5}", FirstName, LastName, DateOfBirth.ToShortDateString(), Age, Email, Nickname);
        }

        public static explicit operator UserVM(UserDTO data)
        {
            return new UserVM { Id = data.Id, FirstName = data.FirstName, LastName = data.LastName, Nickname = data.Nickname, Email = data.Email, DateOfBirth = data.DateOfBirth, HashOfPassword = data.HashOfPassword };
        }
        public static implicit operator UserDTO(UserVM data)
        {
            return new UserDTO() { Id = data.Id, FirstName = data.FirstName, LastName = data.LastName, Nickname = data.Nickname, Email = data.Email, DateOfBirth = data.DateOfBirth, HashOfPassword = data.HashOfPassword };
        }

        public static IEnumerable<UserVM> GetAllUsers()
        {
            List<UserVM> list = new List<UserVM>();
            foreach (var user in usersLogic.GetAllUsers())
            {
                list.Add((UserVM)user);
            }
            return list;
        }

        internal static bool IsValid(string nickname, string password)
        {
            var user = usersLogic.GetAllUsers().FirstOrDefault(x => x.Nickname == nickname);
            if (user != null)
            {
                var hash = user.HashOfPassword;
                var data = Encoding.UTF8.GetBytes(password);
                hashFunction.ComputeHash(data);
                for (int i = 0; i < hash.Length; i++)
                {
                    if (data[i] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        internal static Guid GetUserIdByNickname(string nickname)
        {
            var user = GetAllUsers().FirstOrDefault(x => x.Nickname == nickname);
            if (user != null)
            {
                return user.Id;
            }
            return Guid.Empty;
        }
    }
}