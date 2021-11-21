namespace SocialNetwork.Model.Accounts.Registrations
{
    interface IRegistration
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }

    }
}
