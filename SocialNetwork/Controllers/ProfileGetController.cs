using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Accounts;
using SocialNetwork.Accounts.Profiles;
using SocialNetwork.Data;

namespace SocialNetwork.Controllers
{
    [Route("WTentakle/Profile")]
    public class ProfileGetController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProfileGetController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET WTentakle/Profile
        [HttpGet("{id}")]
        public ActionResult<Profile> GetProfiles(int id)
        {
            if (!_dbContext.AccountDb.Any(i => i.Id == id))
            {
                return NotFound();
            };

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
    }
}
