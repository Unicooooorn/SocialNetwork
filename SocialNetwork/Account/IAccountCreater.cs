namespace SocialNetwork.Account
{
    interface IAccountCreater
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}
