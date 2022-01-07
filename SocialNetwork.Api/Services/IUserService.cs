using System.Threading.Tasks;

namespace SocialNetwork.Api.Services
{
    public interface IUserService
    {
        public Task<string> GetRolesAsync(string login);
    }
}
