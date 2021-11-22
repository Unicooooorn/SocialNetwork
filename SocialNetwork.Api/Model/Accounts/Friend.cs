namespace SocialNetwork.Api.Model.Accounts
{
    public class Friend
    {
        public long FirstAccountId { get; set; }
        public Account FirstAccount { get; set; }

        public long SecondAccountId { get; set; }
        public Account SecondAccount { get; set; }

        public bool IsFriend { get; set; }
    }
}