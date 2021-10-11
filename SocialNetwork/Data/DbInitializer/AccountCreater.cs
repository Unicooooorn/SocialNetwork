using SocialNetwork.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace SocialNetwork.Accounts.DbInitializer
{
    public class AccountCreater
    {
        public static void Initialized(AppDbContext context)
        {
            if (!context.AccountDb.Any())
            {
                context.AccountDb.Add(
                   new Account
                   {
                       Id = 1,
                       FirstName = "Иван",
                       LastName = "Иванов",
                       Password = "1111",
                       DateOfBirth = new DateTime(1996, 4, 4),
                       DateOfRegistration = DateTime.Now

                   });
                context.SaveChanges();
            }
            
        }
    }
}
