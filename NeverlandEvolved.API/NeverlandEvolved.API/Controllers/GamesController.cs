using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Infrastructure.Data;

namespace NeverlandEvolved.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GamesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Games (Hämtar alla spel)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.ToListAsync();
        }

        // POST: api/Games (Skapar ett nytt spel)
        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGames), new { id = game.Id }, game);
        }
    }
}