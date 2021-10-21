using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Rigistration
{
    interface IRegistrable
    {
        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password{ get; set; }

        public DateTime DateOfBirth { get; set; }

        public void RegistrationAcc(string login, string firstName, string lastName, string password, DateTime dateOfBirth) { }
    }
}
