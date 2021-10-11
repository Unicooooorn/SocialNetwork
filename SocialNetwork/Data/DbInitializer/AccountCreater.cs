using SocialNetwork.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Accounts.DbInitializer
{
    public class AccountCreater
    {
        private static AppDbContext _context1 = new AppDbContext();
        static AccountCreater()
        {
            Initialized(_context1);
        }


        public static void Initialized(AppDbContext context)
        {
            if (context.Accounts.Any())
            {
                context.Accounts.Add(
                   new Account
                   {
                       Id = 1,
                       FirstName = "Иван",
                       LastName = "Иванов",
                       Age = 25,
                       Password = "1111"
                   });
                context.SaveChanges();
            }
            
        }
    }
}
