﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Dto.Account;
using SocialNetwork.Api.Model.Accounts;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Api.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SocialNetwork.Api.Controllers
{
    [Route("Wtentakle")]
    public class AccountController : ControllerBase
    {
        public AccountController(AccDbContext dbContext, 
                                ILogger<AccountController> logger, 
                                IConfiguration configuration,
                                IUserService userService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _userService = userService;
        }

        private readonly AccDbContext _dbContext;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        //POST WTentakle/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModelDto model)
        {
            if (ModelState.IsValid)
            {
                Account account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Login == model.Login);

                if (account != null)
                {
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
                SHA256 sha256 = SHA256.Create();

                int salt = new Random().Next();

                Account account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Login == registration.Login);

                if(account == null)
                {
                    account = new Account
                    {
                        Login = registration.Login,
                        FirstName = registration.FirstName,
                        LastName = registration.LastName,
                        Password = Encoding.UTF32.GetString(sha256.ComputeHash(Encoding.UTF32.GetBytes(registration.Password + salt))),
                        DateOfBirth = registration.DateOfBirth.Date,
                        DateOfRegistration = DateTime.Now.Date,
                        Salt = salt,
                        Role = 3
                    };
                    _dbContext.Accounts.Add(account);
                    await _dbContext.SaveChangesAsync();

                    return Ok();
                }
            }
            return BadRequest();
        }

        //POST WTentakle/GetSalt
        [HttpPost("GetSalt")]
        public async Task<IActionResult> GetSalt(string login)
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
                .GetBytes(_configuration.GetSection("Tokens:SECRET_KEY").Value)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "ISSUER",
                "AUDIENCE",
                claims,
                notBefore: DateTime.Now,
                expires:DateTime.Now.AddMinutes(15),
                signingCredentials);

            if (token.Equals(null))
                return BadRequest();

            return Ok();
        }
    }
}
