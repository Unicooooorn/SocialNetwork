using System.Collections.Generic;

namespace SocialNetwork.Model.Accounts
{
    interface IAccount
    {
        public long Id { get; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string DateOfBirth { get; set; }

        public string DateOfRegistration { get; set; }

        public int Salt { get; set; }

        public List<long> Friend { get; set; }
    }
}
