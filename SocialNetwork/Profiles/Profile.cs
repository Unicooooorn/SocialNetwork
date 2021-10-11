using SocialNetwork.Accounts;
using SocialNetwork.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SocialNetwork.Profiles
{
    
    public class Profile : IProfile
    {
        private AppDbContext dbContext = new AppDbContext();

        public int Id { get; set; }
                
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

    }
}
