using Microsoft.AspNetCore.Mvc;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces; // <-- Vi använder nu ditt Interface!

namespace NeverlandEvolved.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameRepository _repository; // <-- Databasen är borta, repot är här!

        public GamesController(IGameRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _repository.GetAllAsync();
            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _repository.GetByIdAsync(id);

            if (game == null) return NotFound();

            return Ok(game);
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame(Game game)
        {
            var createdGame = await _repository.AddAsync(game);
            return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, Game game)
        {
            if (id != game.Id) return BadRequest();

            await _repository.UpdateAsync(game);
            return NoContent();
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _repository.GetByIdAsync(id);
            if (game == null) return NotFound();

            await _repository.DeleteAsync(game);
            return NoContent();
        }
    }
}