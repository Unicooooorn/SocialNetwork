namespace SocialNetwork.Api.Dto.Account
{
    public class ProfileAccountRequest
    {
        public string Login { get; set; }

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
