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
        public int Age { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению!")]
        public string Password { get; set; }

    }
}
