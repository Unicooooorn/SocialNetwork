using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Model.Messages
{
    public class Message : IMessage
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long SenderId { get; set; }

        [Required]
        public long ReceivingId { get; set; }

        [Required]
        public string SentMessage {get; set;} 

        public bool IsReaded { get; set; }  
    }
}
