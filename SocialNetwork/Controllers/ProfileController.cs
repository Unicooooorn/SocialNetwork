using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Accounts;
using SocialNetwork.Accounts.Profiles;
using SocialNetwork.Accounts.Registrations;
using SocialNetwork.Data;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SocialNetwork.Controllers
{
    [Route("WTentakle")]
    public class ProfileController : Controller
    {   
        public ProfileController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly AppDbContext _dbContext;

        //GET WTentakle/Profile
        [HttpGet("Profile/{id}")]
        public ActionResult<Profile> GetProfiles(int id)
        {
            if (!_dbContext.AccountDb.Any(i => i.Id == id))
                return NotFound();

            Account account = (from prof in _dbContext.AccountDb
                               where prof.Id == id
                               select prof).FirstOrDefault();

            int now = int.Parse(DateTime.Now.ToString("yyyy"));
            int dob = int.Parse(account.DateOfBirth.Remove(4, 4));

            Profile profile = new Profile
            {
                Id = account.Id,
                Login = account.Login,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Age = (now - dob)
            };
            return profile;
        }

        //POST WTentakle/Registration/
        [HttpPost("Registration")]
        public ActionResult RegistrationAcc([FromBody] Registration registration)
        {
            Random rnd = new Random();

            SHA256 _SHA256 = SHA256.Create();

            int _salt = rnd.Next();

            if (registration == null || registration.Login == null)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            if (_dbContext.AccountDb.Any(l => l.Login == registration.Login))
                return StatusCode((int)HttpStatusCode.Conflict);

            Account account = new Account
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
            _dbContext.SaveChanges();

            return Ok();
        }

        //PUT Wtentakle/UpdateInfo
        [HttpPut("UpdateInfo/{id}")]
        public ActionResult UpdateInfoAcc([FromBody] Registration registration)
        {
            Account account = (from acc in _dbContext.AccountDb where acc.Login == registration.Login
                              select acc).FirstOrDefault();
            if (account == null)
                return BadRequest();

            account.FirstName = registration.FirstName ?? account.FirstName;
            account.LastName = registration.LastName ?? account.LastName;
            account.DateOfBirth = registration.DateOfBirth ?? account.DateOfBirth;
            account.Login = registration.Login ?? account.Login;

            if (registration.Password != null)
            {
                int salt = account.Salt;
                SHA256 _SHA256 = SHA256.Create();
                account.Password = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(registration.Password + salt.ToString())));
            }

            _dbContext.AccountDb.Update(account);
            _dbContext.SaveChanges();

            return Ok();
        }


        //Delete Wtentakle/DeleteAcc
        [HttpDelete("DeleteAcc/{id}")]
        public ActionResult DeleteAcc([FromBody]string Pass, [FromRoute]int id)
        {            
            if(Pass == null)
                return BadRequest();

            SHA256 _SHA256 = SHA256.Create();
            Account account = (from acc in _dbContext.AccountDb where acc.Id == id select acc).FirstOrDefault();

            string pass = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(Pass + account.Salt)));

            if (pass.Contains(account.Password))
            {
                _dbContext.AccountDb.Remove(account);
                _dbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
