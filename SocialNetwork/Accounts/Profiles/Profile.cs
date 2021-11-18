﻿using SocialNetwork.Data;

namespace SocialNetwork.Accounts.Profiles
{

    public class Profile : IProfile
    {
        public string Login { get; set; }

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
