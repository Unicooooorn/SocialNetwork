using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Accounts;
using SocialNetwork.Accounts.Registrations;
using SocialNetwork.Data;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SocialNetwork.Controllers
{
    [Route("WTentakle/Registration")]
    public class ProfilePostController : Controller
    {   
        private readonly AppDbContext _dbContext;

        public ProfilePostController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //POST WTentakle/Registration
        [HttpPost]
        public ActionResult RegistrationAcc([FromBody] Registration registration)
        {
            Random rnd = new Random();

            SHA256 _SHA256 = SHA256.Create();

            int _salt = rnd.Next();

            if (registration == null || registration.Login == null)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            if (_dbContext.AccountDb.Any(l => l.Login == registration.Login))
                return StatusCode((int)HttpStatusCode.Conflict);

            byte[] pass = Encoding.UTF32.GetBytes(_salt.ToString() + registration.Password);

            Account account = new Account
            {
                Login = registration.Login,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Password = Encoding.UTF32.GetString(_SHA256.ComputeHash(pass)),
                DateOfBirth = registration.DateOfBirth,
                DateOfRegistration = DateTime.Now.ToLongDateString(),
                Salt = _salt
            };
            _dbContext.AccountDb.Add(account);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
