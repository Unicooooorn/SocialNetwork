using Microsoft.EntityFrameworkCore;
using SocialNetwork.Api.Model.Accounts;
using SocialNetwork.Api.Model.Messages;

namespace SocialNetwork.Api.Data
{
    public class AccDbContext : DbContext
    {
        public AccDbContext(DbContextOptions<AccDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Friend> Friends { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasIndex(u => u.Login).IsUnique();

            modelBuilder.Entity<Account>(
                m =>
                {
                    m.Property(b => b.DateOfBirth).HasColumnType("date");
                    m.Property(b => b.DateOfRegistration).HasColumnType("date");
                });

            modelBuilder.Entity<Friend>().HasKey(f => new { f.FirstAccountId, f.SecondAccountId });
        }
    }
}