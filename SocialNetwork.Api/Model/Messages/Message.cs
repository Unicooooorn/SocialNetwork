using SocialNetwork.Api.Model.Accounts;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Api.Model.Messages
{
    public class Message
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long SenderId { get; set; }
        public Account Sender { get; set; }

        [Required]
        public long ReceiverId { get; set; }
        public Account Receiver { get; set; }

        [Required]
        public string Text { get; set; }

        public  DateTime? ReadDateTime { get; set; }
    }
}