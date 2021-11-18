using System;

namespace SocialNetwork.Accounts
{
    interface IAccount
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string DateOfBirth { get; set; }

        public string DateOfRegistration { get; set; }

        public int Salt { get; set; }
    }
}
