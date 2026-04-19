using Microsoft.EntityFrameworkCore;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;
using NeverlandEvolved.Infrastructure.Data;

namespace NeverlandEvolved.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<Game> AddAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task UpdateAsync(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
    }
}