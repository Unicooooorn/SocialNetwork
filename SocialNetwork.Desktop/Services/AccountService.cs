using System;
using SocialNetwork.Api.Dto.Account;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.Mvc;

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

                var pass = loginModelDto.Password;

                loginModelDto.Password = Encoding.ASCII.GetString(
                    SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(pass + await GetSalt(loginModelDto.Login))));

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

            registrationAccountRequest.Salt = new Random().Next();

            var pass = registrationAccountRequest.Password;

            registrationAccountRequest.Password = Encoding.ASCII.GetString(
                SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(pass + registrationAccountRequest.Salt)));

            using var client = new HttpClient();

            var request = await client.PostAsync("https://localhost:5001/WTentakle/Registration",
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

        public async Task<int?> GetSalt(string login)
        {
            using var client = new HttpClient();

            var response =
                await client.GetAsync($"https://localhost:5001/WTentakle/GetSalt/{login}");
            if (response.IsSuccessStatusCode)
            {
                return Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
            }

            return (int?)null;
        }
    }
}
