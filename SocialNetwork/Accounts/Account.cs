using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Accounts
{
    public class Account : IAccount
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле Логин не может быть пустым!")]
        [RegularExpression("[a-zA-Z]", ErrorMessage = "Только латиница!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Укажите дату рождения")]
        public string DateOfBirth { get; set; }

        [Required]
        public string DateOfRegistration { get; set; }

        [Required]
        public int Salt { get; set; }
    }
}
