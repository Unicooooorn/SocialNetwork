using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Account
{
    public class CreateAccount : IAccountCreater
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [RegularExpression(@"[a-zA-Z]", ErrorMessage = "Неверный формат данных")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [RegularExpression(@"[a-zA-Z]", ErrorMessage = "Неверный формат данных")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [RegularExpression(@"[0-9]", ErrorMessage = "Неверный формат данных")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [MaxLength(30)]
        public string Password { get; set; }

    }
}
