using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Models
{
    public class RegisterVM
    {
        [Required]
        [Remote("CheckEmail","Register",ErrorMessage ="Такая почта уже есть")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9_-]+[a-zA-Z0-9]$")]
        [Remote("CheckNickname","Register",ErrorMessage = "Такой Nickname уже есть")]
        public string Nickname { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Zа-яА-Я]+((-[a-zA-Zа-яА-Я]+)+)|([a-zA-Zа-яА-Я]+)$")]
        public string FirstName { get; set; }
        [MinLength(3)]
        [RegularExpression("^[a-zA-Zа-яА-Я]+((-[a-zA-Zа-яА-Я]+)+)|([a-zA-Zа-яА-Я]+)$")]
        public string LastName { get; set; }                 
        public DateTime DateOfBirth { get; set; }
    }
}