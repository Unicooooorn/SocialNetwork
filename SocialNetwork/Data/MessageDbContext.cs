using Microsoft.EntityFrameworkCore;
using SocialNetwork.Model.Messages;

namespace SocialNetwork.Data
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext() { }

        public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options) { } 

        public DbSet<Message> MessageDb { get; set; }

        public object Message { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MessageDb;Username=postgres;Password=1111");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasKey(i => i.Id);
            modelBuilder.Entity<Message>().HasIndex(i => i.Id).IsUnique();
        }
    }
}
