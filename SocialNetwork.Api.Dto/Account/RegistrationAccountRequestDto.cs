namespace SocialNetwork.Api.Dto.Account
{
    public class RegistrationAccountRequestDto
    {
        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Salt { get; set; }
    }
}