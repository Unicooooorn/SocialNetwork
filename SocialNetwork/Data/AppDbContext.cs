using Microsoft.EntityFrameworkCore;
using SocialNetwork.Accounts;

namespace SocialNetwork.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> AccountDb { get; set; }

        public object Account { get; private set; }

        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ProfileDb;Username=postgres;Password=1111");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(u => u.Id);
            modelBuilder.Entity<Account>().HasIndex(u => u.Login).IsUnique();
        }
    }
}
