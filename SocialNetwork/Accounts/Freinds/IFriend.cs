namespace SocialNetwork.Accounts.Freinds
{
    public interface IFriend
    {
        public long MyAccount { get; set; }

        public long FriendAccount { get; set; }
    }
}
