using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SocialNetwork.Model.Logins
{
    public class LoginModel : ILoginModel
    {
        [JsonPropertyName("Login")]
        [Required(ErrorMessage = "Не указан логин!")]
        public string Login { get; set; }

        [JsonPropertyName("Password")]
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
