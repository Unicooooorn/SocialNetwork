using SocialNetwork.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using SocialNetwork.Rigistration;

namespace SocialNetwork.Accounts.DbInitializer
{
    public class AccountCreater : Registrator
    {
        public AccountCreater() : base()
        {
                RegistrationAcc(account);
        }
        
        Account account = new Account
        {
            Login = "Pop",
            FirstName = "Иван",
            LastName = "Иванов",
            Password = "1111",
            DateOfBirth = new DateTime(1996, 4, 4),
            DateOfRegistration = DateTime.Now
        };
    }
}
