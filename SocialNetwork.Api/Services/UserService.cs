using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Dto.Account;

namespace SocialNetwork.Api.Services
{
    public class UserService : IUserService
    {
        private readonly AccDbContext _dbContext;

        public UserService(AccDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetRolesAsync(string login)
        {
            var acc = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Login == login);
            return Enum.GetName((RolesDto) acc.Role);
        }
    }
}
