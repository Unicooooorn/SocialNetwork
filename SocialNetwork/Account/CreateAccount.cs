using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Account
{
    public class CreateAccount : IAccountCreater
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Password { get; set; }

    }
}
