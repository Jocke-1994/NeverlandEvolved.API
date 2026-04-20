using Microsoft.EntityFrameworkCore;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;
using NeverlandEvolved.Infrastructure.Data;

namespace NeverlandEvolved.Infrastructure.Repositories
{
    // ReviewRepository är den konkreta implementationen av IReviewRepository.
    // Den hanterar all direktkommunikation med databasen via EF Core (AppDbContext).
    // Klassen registreras i Program.cs och injiceras via Dependency Injection.
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lägger till recensionen i databasen och returnerar det sparade objektet
        // (som nu har fått sitt Id genererat av databasen).
        public async Task<Review> AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        // Kontrollerar om ett spel med angivet ID finns i databasen.
        // AnyAsync är mer effektivt än FindAsync eftersom det inte hämtar hela objektet.
        public async Task<bool> GameExistsAsync(int gameId)
        {
            return await _context.Games.AnyAsync(g => g.Id == gameId);
        }

        // Kontrollerar om en användare med angivet ID finns i databasen.
        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}
