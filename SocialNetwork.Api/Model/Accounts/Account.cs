using System;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace SocialNetwork.Api.Model.Accounts
{
    public class Account
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfRegistration { get; set; }

        public int Salt { get; set; }

        public int Role { get; set; }
    }
}