using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Model.Accounts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Extensions
{
    public static class CheckFriendExtensions
    {
        private static readonly AppDbContext _dbContext = new();

        public async static Task CheckFriendAsync(this Account account)
        {
            foreach (var item in account.Friend.ToList())
            {
                Account accnt = await _dbContext.AccountDb.FirstOrDefaultAsync(x => x.Id == item);
                if (accnt == null)
                    account.Friend.Remove(item);
            }
            _dbContext.Update(account);
        }
    }
}
