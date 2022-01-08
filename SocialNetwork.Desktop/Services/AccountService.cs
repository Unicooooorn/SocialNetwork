using SocialNetwork.Api.Dto.Account;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SocialNetwork.Desktop.Services
{
    public class AccountService : IAccountService
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

                var salt = GetSalt(loginModelDto.Login);

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

        public async Task<HttpStatusCode> Registration(RegistrationAccountRequestDto registrationAccountRequest)
        {
            foreach (PropertyInfo reg in registrationAccountRequest.GetType().GetProperties())
            {
                if (reg.GetValue(registrationAccountRequest) == null)
                {
                    MessageBox.Show($"{reg.Name} cannot empty");
                    return HttpStatusCode.BadRequest;
                }
            }

            using var client = new HttpClient();

            HttpResponseMessage request = await client.PostAsync("https://localhost:5001/WTentakle/Registration",
                JsonContent.Create(registrationAccountRequest));
            if (request.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show(
                    $"Error:\nStatus Code: {request.StatusCode}\nReason Phrase: {request.ReasonPhrase}");
                return request.StatusCode;
            }

            if (request.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show("Done");
            }
            return HttpStatusCode.OK;
        }

        public async Task<int> GetSalt(string login)
        {
            using var client = new HttpClient();

            HttpResponseMessage response =
                await client.PostAsync("https://localhost:5001/WTentakle/GetSalt", JsonContent.Create(login));

            return 0;
        }
    }
}
