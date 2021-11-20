using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Accounts;
using SocialNetwork.Accounts.Profiles;
using SocialNetwork.Accounts.Registrations;
using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<Profile> GetProfiles(long id)
        {
            Account account = (from prof in _dbContext.AccountDb
                               where prof.Id == id
                               select prof).FirstOrDefault();
            try
            {
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
            catch (NullReferenceException)
            {
                    return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //POST WTentakle/Registration/
        [HttpPost("Registration")]
        public ActionResult RegistrationAcc([FromBody] Registration registration)
        {
            Random rnd = new Random();

            SHA256 _SHA256 = SHA256.Create();

            int _salt = rnd.Next();

            try
            {
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
            catch (Exception)
            {
                if (_dbContext.AccountDb.Any(l => l.Login == registration.Login))
                    return StatusCode(409);

                return BadRequest();
            }            
        }

        //PUT Wtentakle/UpdateInfo
        [HttpPut("UpdateInfo/{id}")]
        public ActionResult UpdateInfoAcc([FromBody] Registration registration)
        {
            Account account = (from acc in _dbContext.AccountDb where acc.Login == registration.Login
                              select acc).FirstOrDefault();

            try
            {
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
            catch (Exception)
            {
                return BadRequest();
            }            
        }


        //Delete Wtentakle/DeleteAcc
        [HttpDelete("DeleteAcc/{id}")]
        public ActionResult DeleteAcc([FromBody]string Pass, [FromRoute]long id)
        {            
            if(Pass == null)
                return BadRequest();

            SHA256 _SHA256 = SHA256.Create();
            Account account = (from acc in _dbContext.AccountDb where acc.Id == id select acc).FirstOrDefault();

            try
            {
                string pass = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(Pass + account.Salt)));

                if (pass.Contains(account.Password))
                {
                    _dbContext.AccountDb.Remove(account);
                    _dbContext.SaveChanges();
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }            
        }

        //GET Wtentakle/GetFriend/
        [HttpGet("GetFriend/{id}")]
        public ActionResult<List<Profile>> GetFriend(long id)
        {
            Account account = (from acc in _dbContext.AccountDb where acc.Id == id select acc).FirstOrDefault();

            try
            {
                List<Profile> prof = new List<Profile>();
                if (account.Friend.Count > 0)
                    foreach (var item in account.Friend)
                    {
                        Account acnt = (from acc in _dbContext.AccountDb where acc.Id == item select acc).FirstOrDefault();
                        if (acnt != null)
                        {
                            int now = int.Parse(DateTime.Now.ToString("yyyy"));
                            int dob = int.Parse(acnt.DateOfBirth.Remove(4, 4));

                            prof.Add(new Profile
                            {
                                FirstName = acnt.FirstName,
                                LastName = acnt.LastName,
                                Id = acnt.Id,
                                Age = (now - dob),
                                Login = acnt.Login
                            });
                        }
                    }
                return prof;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        //POST Wtentakle/AddFriend/
        [HttpPost("AddFriend/{myAccount}")]
        public ActionResult AddFriend([FromBody]long friendAcc, [FromRoute]long myAccount)
        {
            Account account = (from acc in _dbContext.AccountDb where acc.Id == myAccount select acc).FirstOrDefault();

            if(friendAcc.Equals(myAccount))
                return BadRequest();

            try
            {
                if (!account.Friend.Exists(x => x == friendAcc))
                {
                    account.Friend.Add(friendAcc);
                    _dbContext.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
