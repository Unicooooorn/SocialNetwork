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

        public async static Task CheckFriendAsync(this List<long> profiles)
        {
            foreach (var item in profiles.ToList())
            {
                Account account = await _dbContext.AccountDb.FirstOrDefaultAsync(x => x.Id == item);
                if (account == null)
                    profiles.Remove(item);
            }
        }
    }
}
