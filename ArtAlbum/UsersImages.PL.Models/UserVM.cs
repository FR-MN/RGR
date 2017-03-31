using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;

namespace UsersImages.PL.Models
{
    public class UserVM
    {
        public Guid Id { get; private set; }
        private string firstName;// проверки на пустое имя и огромное имя
        private string lastName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) && value.Length > 300)
                {
                    throw new ArgumentException("incorrect title");
                }
                firstName = value;
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) && value.Length > 300)
                {
                    throw new ArgumentException("incorrect title");
                }
                lastName = value;
            }
        }
        public string Nickname { get; set; }


        public string Email { get; set; }
        public int HashOfPassword { get; set; }// что с ним делать?

        private DateTime dateOfBirth;

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                if (value >= DateTime.Now || value.Year < 1900)
                {
                    throw new ArgumentException("incorrect date of birth");
                }
                dateOfBirth = value;
            }
        }


        public UserVM(string firstName, string lastName,string nickname ,string email, DateTime dateOfBirth)
        {

            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
            Email = email;
            DateOfBirth = dateOfBirth;
            Id = Guid.NewGuid();
            
        }

        public UserVM(Guid id, string firstName, string lastName, string nickname, string email, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
            Email = email;
            DateOfBirth = dateOfBirth;
            if (id == null)
            {
                throw new ArgumentNullException("id is null");
            }
            Id = id;
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

        public override string ToString()
        {
            return string.Format("FirstName: {0} , LastName: {1}, date of birth: {2}, age: {3}", FirstName,LastName, dateOfBirth.ToShortDateString(), Age);
        }

        public static explicit operator UserVM(UserDTO data)
        {
            return new UserVM(data.Id, data.FirstName, data.LastName, data.Nickname, data.Email, data.DateOfBirth);
        }

        public static implicit operator UserDTO(UserVM data)
        {
            return new UserDTO() { Id = data.Id, FirstName = data.FirstName ,Nickname= data.Nickname,Email = data.Email, DateOfBirth = data.DateOfBirth };
        }
    }
}
