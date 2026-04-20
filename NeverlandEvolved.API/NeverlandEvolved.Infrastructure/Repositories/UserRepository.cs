using Microsoft.EntityFrameworkCore;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;
using NeverlandEvolved.Infrastructure.Data;

namespace NeverlandEvolved.Infrastructure.Repositories
{
    // UserRepository implementerar IUserRepository och hanterar
    // all databaslogik för User-entiteten via EF Core.
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Hämtar alla användare från databasen.
        // Används bland annat av AuthController för att validera inloggning.
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Hämtar en specifik användare via Id.
        // Returnerar null om användaren inte hittas.
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Lägger till en ny användare i databasen.
        // Returnerar användaren med det Id som databasen genererade.
        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
