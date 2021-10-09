﻿using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Profiles
{
    public class Profile : IProfile
    {
        public int Id { get; set; }
                
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

    }
}
