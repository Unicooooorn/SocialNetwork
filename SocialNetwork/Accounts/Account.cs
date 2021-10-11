using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Accounts
{
    public class Account : IAccount
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Укажите дату рождения")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime DateOfRegistration { get; set; }
    }
}
