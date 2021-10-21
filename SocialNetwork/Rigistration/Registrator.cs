using System;
using SocialNetwork.Data;
using SocialNetwork.Accounts;
using System.Security.Cryptography;
using System.Text;

namespace SocialNetwork.Rigistration
{

    public class Registrator : Account
    {
        Random rnd = new Random();

        private SHA256 _SHA256 = SHA256.Create();

        private int _salt;

        private readonly AppDbContext _dbContext = new AppDbContext();

        public Registrator()
        {
            _salt = rnd.Next();
        }
                       

        public void RegistrationAcc(Account account)
        {
            byte[] pass = Encoding.UTF32.GetBytes(_salt.ToString() + account.Password);

            account = new Account
            {
                Login = account.Login,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Password = Encoding.UTF32.GetString(_SHA256.ComputeHash(pass)),
                DateOfBirth = account.DateOfBirth,
                DateOfRegistration = DateTime.Now,
                Salt = _salt
            };
            _dbContext.AccountDb.Add(account);
            _dbContext.SaveChanges();
        }
    }
}
