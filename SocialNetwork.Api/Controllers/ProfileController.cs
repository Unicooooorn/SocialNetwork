using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Dto.Account;
using SocialNetwork.Api.Model.Accounts;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Api.Controllers
{
    [Route("WTentakle")]
    public class ProfileController : ControllerBase
    {
        public ProfileController(AccDbContext dbContext, ILogger<ProfileController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        private readonly AccDbContext _dbContext;
        private readonly ILogger<ProfileController> _logger;

        //GET WTentakle/Profile
        [HttpGet("Profile/{id}")]
        public async Task<ActionResult<ProfileAccountRequest>> GetProfilesAsync(long id)
        {
            Account account = await _dbContext.Accounts.FirstOrDefaultAsync(account => account.Id == id);

            ProfileAccountRequest profile = new ProfileAccountRequest()
            {
                Id = account.Id,
                Login = account.Login,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Age = (DateTime.Now - account.DateOfBirth).Days / 365
            };
            return Ok(profile);
        }

        //PUT Wtentakle/UpdateInfo
        [HttpPut("UpdateInfo/{id}")]
        public async Task<ActionResult> UpdateInfoAccountAsync([FromBody] UpdateAccountInfo updateInfo)
        {
            Account account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Login == updateInfo.Login);

            account.FirstName = updateInfo.FirstName ?? account.FirstName;
            account.LastName = updateInfo.LastName ?? account.LastName;
            account.Login = updateInfo.Login ?? account.Login;

            if (updateInfo.Password != null)
            {
                int salt = account.Salt;
                SHA256 _SHA256 = SHA256.Create();
                account.Password = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(updateInfo.Password + salt)));
            }

            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }


        //DELETE Wtentakle/DeleteAcc
        [HttpDelete("DeleteAcc/{id}")]
        public async Task<ActionResult> DeleteAccountAsync([FromBody]string Pass, [FromRoute]long id)
        {            
            if(Pass == null)
                return BadRequest();

            SHA256 _SHA256 = SHA256.Create();
            Account account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            if (account == null)
                return NotFound();

            string pass = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(Pass + account.Salt)));

            if (pass.Contains(account.Password))
            {
                _dbContext.Accounts.Remove(account);
                await _dbContext.SaveChangesAsync();
            }
            return Ok();
        }

        //GET Wtentakle/GetFriend/
        [HttpGet("GetFriend/{id}")]
        public async Task<IActionResult> GetFriendAsync(long id)
        {
            List<Friend> friends = new();

            foreach (var friend in _dbContext.Friends)
            {
                if (friend.IsFriend && (friend.FirstAccountId == id || friend.SecondAccountId == id))
                {
                    friends.Add(friend);
                }
            }

            List<ProfileAccountRequest> profileAccount = new();
            foreach (var friend in friends)
            {
                if (friend.FirstAccountId == id)
                {
                    Account account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == friend.SecondAccountId);

                    profileAccount.Add(
                        new ProfileAccountRequest()
                        {
                            Id = account.Id,
                            Login = account.Login,
                            FirstName = account.FirstName,
                            LastName = account.LastName,
                            Age = (DateTime.Now - account.DateOfBirth).Days/365
                        });
                }
                else
                {
                    Account account =
                        await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == friend.FirstAccountId);

                    profileAccount.Add(
                        new ProfileAccountRequest()
                        {
                            Id = account.Id,
                            Login = account.Login,
                            FirstName = account.FirstName,
                            LastName = account.LastName,
                            Age = (DateTime.Now - account.DateOfBirth).Days / 365
                        });
                }
            }


            return Ok(profileAccount);
        }


        //POST Wtentakle/AddFriend/
        [HttpPost("AddFriend/{myAccountId}")]
        public async Task<IActionResult> AddFriendAsync([FromBody] long friendAccountId, [FromRoute] long myAccountId)
        {
            Account myAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(x => x.Id == myAccountId);
            if (myAccount == null)
                return BadRequest();

            if (friendAccountId.Equals(myAccountId))
                return BadRequest();

            var friendAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(x => x.Id == friendAccountId);
            if (friendAccount == null)
                return BadRequest();

            var friends = await _dbContext.Friends
                .FirstOrDefaultAsync( x => x.FirstAccountId == friendAccountId && x.SecondAccountId == myAccountId && x.IsFriend == false);
            if (friends != null)
            {
                friends.IsFriend = true;

                await _dbContext.SaveChangesAsync();

                return Ok();
            }

            var friend = new Friend
            {
                FirstAccount = myAccount, 
                SecondAccount = friendAccount,
                IsFriend = false
            };
            _dbContext.Friends.Add(friend);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }


        ///DELETE Wtentakle/DropFriend
        [HttpDelete("DeleteFriend/{myAccountId}")]
        public async Task<IActionResult> DeleteFriendAsync([FromBody] long friendAccountId, [FromRoute] long myAccountId)
        {
            Friend friend = await _dbContext.Friends
                .FirstOrDefaultAsync((x => 
                    (x.FirstAccountId == myAccountId && x.SecondAccountId == friendAccountId) || 
                    (x.FirstAccountId == friendAccountId && x.SecondAccountId == myAccountId)));

            if (friend == null)
                return NotFound();

            if (friend.IsFriend)
                friend.IsFriend = false;

            await _dbContext.SaveChangesAsync();
            
            return Ok();
        }
    }
}
