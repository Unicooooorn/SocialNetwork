using System.Text.Json.Serialization;

namespace SocialNetwork.Accounts.Registrations
{
    public class Registration : IRegistration
    {
        [JsonPropertyName("Login")]
        public string Login { get; set; }

        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("DateOfBirth")]
        public string DateOfBirth { get; set; }
    }
}
