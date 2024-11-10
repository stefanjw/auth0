using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Seed initial data with a string UserId
            modelBuilder.Entity<User>().HasData(
                new User { UserId = "auth0|123456", Name = "Sample User", Email = "sample@example.com" }
            );
        }
    }
}



