using SocialNetwork.Data;
using System;
using System.Linq;

namespace SocialNetwork.Profiles
{

    public class Profile : IProfile
    {
        private readonly AppDbContext _dbContext;

        public int Id { get; set; }
                
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public Profile()
        {
            Id = (from id in _dbContext.AccountDb select id.Id).FirstOrDefault();
            FirstName = (from fName in _dbContext.AccountDb select fName.FirstName).FirstOrDefault();
            LastName = (from lName in _dbContext.AccountDb select lName.LastName).FirstOrDefault();
            Age = Convert.ToInt32(DateTime.Now - (from dBirth in _dbContext.AccountDb select dBirth.DateOfBirth).FirstOrDefault());
        }

    }
}
