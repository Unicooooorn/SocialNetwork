using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Data;
using SocialNetwork.Accounts;

namespace SocialNetwork.Rigistration
{
    public class Registrator
    {             
        public Registrator(Account account)
        {
            using (AppDbContext dbContext = new AppDbContext())
            {
                dbContext.AccountDb.Add(account);
            }
        }
    }
}
