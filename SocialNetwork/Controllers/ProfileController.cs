using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Model.Accounts;
using SocialNetwork.Model.Accounts.Profiles;
using SocialNetwork.Model.Accounts.Registrations;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Extensions;
using System.Linq;

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
        public async Task<ActionResult<Profile>> GetProfiles(long id)
        {
            Account account = await _dbContext.AccountDb.FirstOrDefaultAsync(account => account.Id == id);
                              
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

        //PUT Wtentakle/UpdateInfo
        [HttpPut("UpdateInfo/{id}")]
        public async Task<ActionResult> UpdateInfoAcc([FromBody] Registration registration)
        {
            Account account = await _dbContext.AccountDb.FirstOrDefaultAsync(x => x.Login == registration.Login);

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
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }            
        }


        //DELETE Wtentakle/DeleteAcc
        [HttpDelete("DeleteAcc/{id}")]
        public async Task<ActionResult> DeleteAcc([FromBody]string Pass, [FromRoute]long id)
        {            
            if(Pass == null)
                return BadRequest();

            SHA256 _SHA256 = SHA256.Create();
            Account account = await _dbContext.AccountDb.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                string pass = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(Pass + account.Salt)));

                if (pass.Contains(account.Password))
                {
                    _dbContext.AccountDb.Remove(account);
                    await _dbContext.SaveChangesAsync();
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
        public async Task<ActionResult<List<Profile>>> GetFriend(long id)
        {
            Account account = await _dbContext.AccountDb.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                List<Profile> prof = new();
                await CheckFriendExtensions.CheckFriendAsync(account.Friend);
                _dbContext.Update(account);
                await _dbContext.SaveChangesAsync();

                if (account.Friend.Count > 0)
                {                    
                    foreach (var item in account.Friend)
                    {
                        Account acnt = await _dbContext.AccountDb.FirstOrDefaultAsync(x => x.Id == item);

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
        public async Task<ActionResult> AddFriend([FromBody]long friendAcc, [FromRoute]long myAccount)
        {
            Account account = await _dbContext.AccountDb.FirstOrDefaultAsync(x => x.Id == myAccount);

            if(friendAcc.Equals(myAccount))
                return BadRequest();

            try
            {
                if (!account.Friend.Exists(x => x == friendAcc))
                {
                    account.Friend.Add(friendAcc);
                    await _dbContext.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        //DELETE Wtentakle/DropFriend
        [HttpDelete("DropFriend/{myAccount}")]
        public async Task<ActionResult> DropFriend([FromBody]long friendAcc, [FromRoute] long myAccount)
        {
            Account account = await _dbContext.AccountDb.FirstOrDefaultAsync(x => x.Id == myAccount);

            await CheckFriendExtensions.CheckFriendAsync(account.Friend);
            _dbContext.Update(account);
            await _dbContext.SaveChangesAsync();

            try
            {
                if (!account.Friend.Exists(f => f == friendAcc))
                    throw new Exception();
                account.Friend.Remove(friendAcc);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
