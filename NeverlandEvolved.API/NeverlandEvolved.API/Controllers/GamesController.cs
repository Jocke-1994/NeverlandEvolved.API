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
        // GET: api/Games/5 (Hämtar ETT specifikt spel)
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound(); // 404 om spelet inte hittas
            }

            return game;
        }

        // PUT: api/Games/5 (Uppdaterar ett spel)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest("ID i URL måste matcha ID i bodyn."); // 400 Bad Request
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Games.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // 204 No Content (Standard när en uppdatering lyckas)
        }

        // DELETE: api/Games/5 (Raderar ett spel)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
    }
}