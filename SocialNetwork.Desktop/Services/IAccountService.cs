using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Api.Dto.Account;

namespace SocialNetwork.Desktop.Services
{
    public interface IAccountService
    {
        public Task<HttpStatusCode> LoginServiceAsync(LoginModelDto loginModelDto);
        public Task<HttpStatusCode> Registration(RegistrationAccountRequestDto registrationAccountRequest);
        public Task<int?> GetSalt(string login);
    }
}
