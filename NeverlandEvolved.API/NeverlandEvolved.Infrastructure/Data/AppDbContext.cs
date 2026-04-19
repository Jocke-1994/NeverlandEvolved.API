using Microsoft.EntityFrameworkCore;
using NeverlandEvolved.Domain.Entities; // Importerar dina modeller från Domain

namespace NeverlandEvolved.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Dessa DbSets blir dina tabeller i SQL-databasen
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasPrecision(18, 2); // Betyder: Max 18 siffror, varav 2 decimaler
        }
    }
}