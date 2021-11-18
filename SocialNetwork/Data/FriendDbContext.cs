using Microsoft.EntityFrameworkCore;
using SocialNetwork.Accounts.Freinds;

namespace SocialNetwork.Data
{
    public class FriendDbContext : DbContext
    {
        public FriendDbContext() { }

        public FriendDbContext(DbContextOptions<FriendDbContext> options) : base(options) { }

        public DbSet<Friend> FriendDb { get; set; }

        public object Friend { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ProfileDb;Username=postgres;Password=1111");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>().HasNoKey();
        }
    }
}
