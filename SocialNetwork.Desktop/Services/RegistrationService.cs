using System.Linq;
using SocialNetwork.Api.Dto.Account;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SocialNetwork.Desktop.Services
{
    public class RegistrationService
    {
        public async Task<HttpStatusCode> Registration(RegistrationAccountRequest registrationAccountRequest)
        {
            if (registrationAccountRequest != null)
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

                return HttpStatusCode.OK;
            }
            return HttpStatusCode.NotFound;
        }
    }
}
