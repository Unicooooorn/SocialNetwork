using Microsoft.EntityFrameworkCore;
using SocialNetwork.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CreateAccount> Accounts { get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=ProfileDb;Username=postgres;Password=1111");
        }
    }
}
