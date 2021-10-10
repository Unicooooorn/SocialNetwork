using SocialNetwork.Accounts;
using SocialNetwork.Accounts.DbInitializer;

namespace SocialNetwork.Data.DbInitializer
{
    public class FirstProfile : AccountCreater
    {
        private readonly AppDbContext _context = new AppDbContext();
        public FirstProfile()
        {
            Initialized(_context);
        }
    }
}
