using SocialNetwork.Api.Dto.Account;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SocialNetwork.Desktop.Services
{
    public static class RegistrationService
    {
        public static async Task<HttpStatusCode> Registration(RegistrationAccountRequestDto registrationAccountRequest)
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
    }
}
