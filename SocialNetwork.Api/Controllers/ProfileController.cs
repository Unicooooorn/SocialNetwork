using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfilesAsync()
        {
            Account account = await _dbContext.Accounts.FirstOrDefaultAsync(account => account.Login == User.Identity.Name);

            ProfileAccountRequestDto profile = new ProfileAccountRequestDto()
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
        [HttpPut("UpdateInfo/")]
        public async Task<IActionResult> UpdateInfoAccountAsync([FromBody] UpdateAccountInfoDto updateInfoDto)
        {
            Account account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Login == User.Identity.Name);

            account.FirstName = updateInfoDto.FirstName ?? account.FirstName;
            account.LastName = updateInfoDto.LastName ?? account.LastName;
            account.Login = updateInfoDto.Login ?? account.Login;

            if (updateInfoDto.Password != null)
            {
                int salt = account.Salt;
                SHA256 _SHA256 = SHA256.Create();
                account.Password = Encoding.UTF32.GetString(_SHA256.ComputeHash(Encoding.UTF32.GetBytes(updateInfoDto.Password + salt)));
            }

            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }


        //DELETE Wtentakle/DeleteAcc
        [HttpDelete("DeleteAcc")]
        public async Task<IActionResult> DeleteAccountAsync([FromBody]string Pass, [FromRoute]long id)
        {            

            if(Pass == null)
                return BadRequest();

            SHA256 _SHA256 = SHA256.Create();
            Account account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Login == User.Identity.Name);

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
        [HttpGet("GetFriend")]
        public async Task<IActionResult> GetFriendAsync()
        {
            List<Friend> friends = new();

            foreach (var friend in _dbContext.Friends)
            {
                if (friend.IsFriend && (friend.FirstAccount.Login == User.Identity.Name || friend.SecondAccount.Login == User.Identity.Name))
                {
                    friends.Add(friend);
                }
            }

            List<ProfileAccountRequestDto> profileAccount = new();
            foreach (var friend in friends)
            {
                if (friend.FirstAccount.Login == User.Identity.Name)
                {
                    Account account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == friend.SecondAccountId);

                    profileAccount.Add(
                        new ProfileAccountRequestDto()
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
                        new ProfileAccountRequestDto()
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
        [HttpPost("AddFriend")]
        public async Task<IActionResult> AddFriendAsync([FromBody] long friendAccountId)
        {
            Account myAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(x => x.Login == User.Identity.Name);
            if (myAccount == null)
                return BadRequest();

            if (friendAccountId.Equals(User.Identity.Name))
                return BadRequest();

            var friendAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(x => x.Id == friendAccountId);
            if (friendAccount == null)
                return BadRequest();

            var friends = await _dbContext.Friends
                .FirstOrDefaultAsync( x => x.FirstAccountId == friendAccountId && x.SecondAccount.Login == User.Identity.Name && x.IsFriend == false);
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
        [HttpDelete("DeleteFriend")]
        public async Task<IActionResult> DeleteFriendAsync([FromBody] long friendAccountId)
        {
            Friend friend = await _dbContext.Friends
                .FirstOrDefaultAsync((x => 
                    (x.FirstAccount.Login == User.Identity.Name && x.SecondAccountId == friendAccountId) || 
                    (x.FirstAccountId == friendAccountId && x.SecondAccount.Login == User.Identity.Name)));

            if (friend == null)
                return NotFound();

            if (friend.IsFriend)
                friend.IsFriend = false;

            await _dbContext.SaveChangesAsync();
            
            return Ok();
        }
    }
}
