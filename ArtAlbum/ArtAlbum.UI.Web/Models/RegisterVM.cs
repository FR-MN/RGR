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
        [Required(ErrorMessage ="Это поле не может быть пустым")]
        [Display(Name ="Электронная почта")]
        [Remote("CheckEmail", "Register", ErrorMessage = "Такая почта уже есть")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Неверный формат почты")]
        [MaxLength(50, ErrorMessage ="Максимальная длина - 50 символов")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [Display(Name = "Ник")]
        [MinLength(4, ErrorMessage ="Минимальная допустимая длина ника - 4 символа")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9-_\.]{0,50}$", ErrorMessage ="Ник может содержать только цифры и буквы латинского алфавита")]
        [Remote("CheckNickname","Register",ErrorMessage = "Такой ник уже есть")]
        [MaxLength(20, ErrorMessage = "Максимальная длина - 20 символов")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [Display(Name = "Пароль")]
        [MinLength(8, ErrorMessage = "Минимальная допустимая длина пароля - 8 символа")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$", ErrorMessage = "Пароль обязательно должен содержать цифру, прописную и строчную букву латинского алфавита")]
        [DataType(DataType.Password)]
        [MaxLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage ="Пароли не совпадают")]
        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [MaxLength(49, ErrorMessage = "Максимальная длина - 50 символов")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [Display(Name = "Имя")]
        [MinLength(2, ErrorMessage = "Минимальная допустимая длина имени - 2 символа")]
        [RegularExpression("^[а-яА-ЯёЁa-zA-Z]+$")]
        [MaxLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [Display(Name = "Фамилия")]
        [MinLength(2, ErrorMessage = "Минимальная допустимая длина фамилии - 2 символа")]
        [RegularExpression("^[а-яА-ЯёЁa-zA-Z]+$")]
        [MaxLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [Remote("CheckDateOfBirth", "Register", ErrorMessage = "Некорректная дата рождения")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}