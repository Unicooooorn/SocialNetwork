using System.Reflection;

namespace SocialNetwork.Accounts.Freinds
{
    public class Friend : IFriend
    {
        public long MyAccount { get; set; }

        public long FriendAccount { get; set; }
    }
}
