using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Dto.Account;
using SocialNetwork.Api.Model.Accounts;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Api.Controllers
{
    [Route("Wtentakle")]
    public class AccountController : ControllerBase
    {
        public AccountController(AccDbContext dbContext, ILogger<AccountController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        private readonly AccDbContext _dbContext;
        private readonly ILogger<AccountController> _logger;

        //POST WTentakle/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody]LoginModelDto model)
        {
            if (ModelState.IsValid)
            {
                SHA256 _SHA256 = SHA256.Create();

                Account account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Login == model.Login);

                if (account != null)
                {
                    string pass = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(model.Password + account.Salt)));
                    if (pass.Contains(account.Password))
                    {
                        await Authenticate(model.Login);
                        return Ok();
                    }

                    return BadRequest();
                }
                return NotFound();
            }
            return BadRequest();
        }


        //POST WTentakle/Registration/
        [HttpPost("Registration")]
        public async Task<ActionResult> RegistrationAccountAsync([FromBody]RegistrationAccountRequest registration)
        {
            if (ModelState.IsValid)
            {
                Random rnd = new Random();

                SHA256 _SHA256 = SHA256.Create();

                int _salt = rnd.Next();

                Account account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Login == registration.Login);

                if(account == null)
                {
                    account = new Account
                    {
                        Login = registration.Login,
                        FirstName = registration.FirstName,
                        LastName = registration.LastName,
                        Password = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(registration.Password + _salt))),
                        DateOfBirth = registration.DateOfBirth.Date,
                        DateOfRegistration = DateTime.Now.Date,
                        Salt = _salt
                    };
                    _dbContext.Accounts.Add(account);
                    await _dbContext.SaveChangesAsync();

                    await Authenticate(registration.Login);

                    return Ok();
                }
            }
            return BadRequest();
        }



        private async Task<IActionResult> Authenticate(string login)
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
