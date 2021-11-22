using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Model.Accounts;
using SocialNetwork.Api.Model.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.Api.Controllers
{
    [Route("WTentakle")]
    public class MessageController : ControllerBase
    {
        public MessageController(AccDbContext appDbContext, ILogger<MessageController> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        private readonly AccDbContext _appDbContext;
        private readonly ILogger<MessageController> _logger;

        //GET RequestMessage
        [HttpGet("Mail/{id}")]
        public async Task<IActionResult> GetMessageAsync([FromRoute] long id)
        {
            List<string> messages = new();

            foreach (var item in _appDbContext.Messages)
            {
                if (item.ReceiverId == id)
                {
                    messages.Add(item.Text);

                    item.ReadDateTime ??= DateTime.UtcNow;
                }
            }

            await _appDbContext.SaveChangesAsync();

            return Ok(messages);
        }


        [HttpPost("Mail/{senderId}-{receiverId}")]
        public async Task<ActionResult> SendMessageAsync([FromBody] string text, [FromRoute] long senderId, [FromRoute] long receiverId)
        {
            Account senderAccount = await _appDbContext.Accounts.FirstOrDefaultAsync(i => i.Id == senderId);
            if (senderAccount == null)
            {
                _logger.LogDebug("Account with {Id} not found", senderId);

                return NotFound();
            }

            Account receiverAccount = await _appDbContext.Accounts.FirstOrDefaultAsync(i => i.Id == receiverId);
            if (receiverAccount == null)
            {
                _logger.LogDebug("Account with {id} not found", receiverId);

                return NotFound();
            }
            
            if (senderId.Equals(receiverId))
                return BadRequest();

            if (text != null)
            {
                Message message = new Message
                {
                    Sender = senderAccount,
                    Receiver = receiverAccount,
                    ReadDateTime = null,
                    Text = text
                };
                _appDbContext.Messages.Add(message);
                await _appDbContext.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }
    }
}
