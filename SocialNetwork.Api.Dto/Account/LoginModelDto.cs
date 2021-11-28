using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Api.Dto.Account
{
    public class LoginModelDto
    {
        [Required(ErrorMessage = "Не указан логин!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
