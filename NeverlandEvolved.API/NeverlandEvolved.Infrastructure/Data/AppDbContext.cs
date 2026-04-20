using Microsoft.EntityFrameworkCore;
using NeverlandEvolved.Domain.Entities;

namespace NeverlandEvolved.Infrastructure.Data
{
    // AppDbContext är kärnan i Entity Framework Core.
    // Den representerar en session mot databasen och låter oss
    // läsa och skriva data via DbSet-egenskaperna nedan.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Varje DbSet<T> motsvarar en tabell i SQL-databasen.
        // EF Core skapar dessa tabeller automatiskt via migrations.
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // OnModelCreating används för att konfigurera tabellstrukturen
        // med Fluent API, t.ex. precision på decimalfält.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Sätter precision för Price-kolumnen: max 18 siffror totalt, varav 2 decimaler.
            // Detta krävs av SQL Server för decimal-typen.
            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasPrecision(18, 2);
        }
    }
}
