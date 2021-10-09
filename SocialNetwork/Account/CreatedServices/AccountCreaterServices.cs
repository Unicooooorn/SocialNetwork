using SocialNetwork.Data;
using SocialNetwork.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Account.CreatedServices
{
    public class AccountCreaterServices : CreateAccount
    {
        public static void Initialized(AppDbContext context)
        {
            if(!context.Accounts.Any())
            {
                context.Accounts.AddRange(
                    new CreateAccount
                    {
                        Id = 1,
                        FirstName = "Иван",
                        LastName = "Иванов",
                        Age = 25,
                        Password = "1111",
                        PasswordConfirm = "1111"
                    });
                context.SaveChanges();
            }
        }
    }
}
