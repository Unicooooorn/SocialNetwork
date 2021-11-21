namespace SocialNetwork.Model.Messages
{
    public interface IMessage
    {
        public long Id { get; set; }

        public long SenderId { get; set; }
        
        public long ReceivingId { get; set; }

        public string SentMessage { get; set; }

        public bool IsReaded { get; set; }
    }
}
