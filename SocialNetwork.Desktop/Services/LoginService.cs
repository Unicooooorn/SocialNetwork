using SocialNetwork.Api.Dto.Account;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace SocialNetwork.Desktop.Services
{
    public class LoginService
    {
        public async Task<HttpStatusCode> LoginServiceAsync(LoginModelDto loginModelDto)
        {
            if (loginModelDto != null)
            {
                if (loginModelDto.Login == null)
                {
                    MessageBox.Show("Login cannot be empty");
                    return HttpStatusCode.BadRequest;
                }

                if (loginModelDto.Password == null)
                {
                    MessageBox.Show("Password cannot be empty");
                    return HttpStatusCode.BadRequest;
                }

                using var client = new HttpClient();

                HttpResponseMessage request = await client.PostAsync("https://localhost:5001/WTentakle/Login",
                    JsonContent.Create(loginModelDto));

                if (request.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show(
                        $"Error:\nStatus Code: {request.StatusCode}\nReason Phrase: {request.ReasonPhrase}");
                    return HttpStatusCode.BadRequest;
                }

                return HttpStatusCode.OK;
            }

            return HttpStatusCode.NotFound;
        }
    }
}
