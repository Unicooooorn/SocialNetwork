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

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ProfileDb;Username=postgres;Password=1111");

    }
}
