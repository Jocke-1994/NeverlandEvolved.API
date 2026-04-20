using Microsoft.EntityFrameworkCore;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;
using NeverlandEvolved.Infrastructure.Data;

namespace NeverlandEvolved.Infrastructure.Repositories
{
    // GameRepository implementerar IGameRepository och hanterar
    // all databaslogik för Game-entiteten via EF Core.
    // Klassen registreras i Program.cs och injiceras via Dependency Injection.
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context)
        {
            _context = context;
        }

        // Hämtar alla spel från databasen som en lista.
        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        // Hämtar ett specifikt spel via primärnyckeln (Id).
        // Returnerar null om spelet inte hittas.
        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        // Lägger till ett nytt spel i databasen.
        // Returnerar spelet med det Id som databasen genererade.
        public async Task<Game> AddAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        // Markerar entiteten som "Modified" så att EF Core vet att den ska uppdateras.
        // SaveChangesAsync skriver sedan ändringarna till databasen.
        public async Task UpdateAsync(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Tar bort ett spel från databasen permanent.
        public async Task DeleteAsync(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
    }
}
