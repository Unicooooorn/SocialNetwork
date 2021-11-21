using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Data;
using SocialNetwork.Model.Accounts;
using SocialNetwork.Model.Messages;
using System.Linq;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Controllers
{
    [Route("WTentakle")]
    public class MessageController : Controller
    {
        public MessageController(MessageDbContext messageDbContext, AppDbContext appDbContext)
        {
            _messageDbContext = messageDbContext;
            _appDbContext = appDbContext;
        }

        private readonly MessageDbContext _messageDbContext;
        private readonly AppDbContext _appDbContext;

        //GET RequestMessage
        [HttpGet("Mail/{id}")]
        public ActionResult<List<string>> GetMessage([FromRoute] long id)
        {
            Message message = (from m in _messageDbContext.MessageDb where m.ReceivingId == id select m).FirstOrDefault();

            if (message == null)
                return NotFound();

            List<string> query = new();
            var allMessage = from m in _messageDbContext.MessageDb where m.ReceivingId == id select m;
            try
            {
                foreach (var item in allMessage)
                {
                    query.Add(item.SentMessage);
                    if(!item.IsReaded)
                        item.IsReaded = true;
                }
                return query;
            }
            catch (Exception)
            {
                return BadRequest();
            }
            finally
            {
                _messageDbContext.SaveChangesAsync();
            }
        }

        [HttpPost("Mail/{myId}-{sentId}")]
        public ActionResult SendMessage([FromBody] string query, [FromRoute] long myId, [FromRoute] long sentId)
        {
            Account checkMyId = (from i in _appDbContext.AccountDb where i.Id == myId select i).FirstOrDefault();
            if (checkMyId == null)
                return NoContent();

            Account checkSentId = (from i in _appDbContext.AccountDb where i.Id == sentId select i).FirstOrDefault();
            if (checkSentId == null)
                return NoContent();
            

            if (myId.Equals(sentId))
                return BadRequest();

            try
            {
                if (query != null)
                {
                    Message message = new Message
                    {
                        SenderId = myId,
                        ReceivingId = sentId,
                        IsReaded = false,
                        SentMessage = query
                    };
                    _messageDbContext.MessageDb.Add(message);
                    _messageDbContext.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
