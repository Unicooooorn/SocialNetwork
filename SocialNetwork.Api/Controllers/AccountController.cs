using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Dto.Account;
using SocialNetwork.Api.Model.Accounts;
using SocialNetwork.Api.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SocialNetwork.Api.Controllers
{
    [Route("Wtentakle")]
    public class AccountController : ControllerBase
    {
        private readonly AccDbContext _dbContext;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment;

        public AccountController(AccDbContext dbContext, 
                                ILogger<AccountController> logger, 
                                IConfiguration configuration,
                                IUserService userService,
                                IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _userService = userService;
            _environment = environment;
        }


        //POST WTentakle/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModelDto model)
        {
            if (ModelState.IsValid)
            {
                Account account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Login == model.Login);

                if (account != null)
                {
                    if (_environment.IsDevelopment())
                    {
                        var pass = Encoding.ASCII.GetString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(model.Password + account.Salt)));

                        if (!pass.Contains(account.Password)) return Unauthorized();

                        Authenticate(model.Login);
                        return Ok();
                    }

                    if (model.Password.Contains(account.Password))
                    {
                        Authenticate(model.Login);
                        return Ok();
                    }
                    return Unauthorized();
                }
                return NotFound();
            }
            return BadRequest();
        }


        //POST WTentakle/Registration/
        [HttpPost("Registration")]
        public async Task<IActionResult> RegistrationAccountAsync([FromBody]RegistrationAccountRequestDto registration)
        {
            if (ModelState.IsValid)
            {
                Account account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Login == registration.Login);

                if(account == null)
                {
                    account = new Account
                    {
                        Login = registration.Login,
                        FirstName = registration.FirstName,
                        LastName = registration.LastName,
                        Password = registration.Password,
                        DateOfBirth = registration.DateOfBirth.Date,
                        DateOfRegistration = DateTime.Now.Date,
                        Salt = registration.Salt,
                        Role = 3
                    };
                    _dbContext.Accounts.Add(account);
                    await _dbContext.SaveChangesAsync();

                    return Ok();
                }
            }
            return BadRequest();
        }

        //GET WTentakle/GetSalt
        [HttpGet("GetSalt/{login}")]
        public async Task<ActionResult<int>> GetSalt([FromRoute]string login)
        {
            if (ModelState.IsValid)
            {
                Account account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Login == login);

                if (account.Equals(null)) 
                    return BadRequest();

                return Ok(account.Salt);
            }

            return BadRequest();
        }

        private IActionResult Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var role = _userService.GetRolesAsync(login);
            
            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Result));

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(_configuration["SECRET_KEY_SN"])), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["ISSUER_SN"],
                _configuration["AUDIENCE_SN"],
                claims,
                notBefore: DateTime.Now,
                expires:DateTime.Now.AddMinutes(15),
                signingCredentials);

            if (token.Equals(null))
                return BadRequest();

            return Ok(token);
        }
    }
}
