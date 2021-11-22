using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SocialNetwork.Api.Model.Logins
{
    public class LoginModel
    {
        [JsonPropertyName("Login")]
        [Required(ErrorMessage = "Не указан логин!")]
        public string Login { get; set; }

        [JsonPropertyName("Password")]
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
