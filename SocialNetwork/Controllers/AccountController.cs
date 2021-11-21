using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Model.Accounts;
using SocialNetwork.Model.Logins;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using SocialNetwork.Model.Accounts.Registrations;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;

namespace SocialNetwork.Controllers
{
    [Route("Wtentakle")]
    public class AccountController : Controller
    {
        public AccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly AppDbContext _dbContext;

        //POST WTentakle/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                SHA256 _SHA256 = SHA256.Create();

                Account account = await _dbContext.AccountDb.FirstOrDefaultAsync(a => a.Login == model.Login);

                if (account != null)
                {
                    string pass = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(model.Password + account.Salt)));
                    if (pass.Contains(account.Password))
                    {
                        await Authenticate(model.Login);
                        return Ok();
                    }
                    
                }
            }
            return BadRequest();
        }


        //POST WTentakle/Registration/
        [HttpPost("Registration")]
        public async Task<ActionResult> RegistrationAcc([FromBody] Registration registration)
        {
            

            if (ModelState.IsValid)
            {
                Random rnd = new Random();

                SHA256 _SHA256 = SHA256.Create();

                int _salt = rnd.Next();

                Account account = await _dbContext.AccountDb.FirstOrDefaultAsync(a => a.Login == registration.Login && a.Password == registration.Password);

                if(account == null)
                {
                    account = new Account
                    {
                        Login = registration.Login,
                        FirstName = registration.FirstName,
                        LastName = registration.LastName,
                        Password = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(registration.Password + _salt.ToString()))),
                        DateOfBirth = registration.DateOfBirth,
                        DateOfRegistration = DateTime.Now.ToLongDateString(),
                        Salt = _salt
                    };
                    _dbContext.AccountDb.Add(account);
                    await _dbContext.SaveChangesAsync();

                    await Authenticate(registration.Login);

                    return Ok();
                }
            }
            return BadRequest();
        }

        private async Task<ActionResult> Authenticate(string login)
        {
            var claims = new List<Claim>();
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login);
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            return Ok();
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
