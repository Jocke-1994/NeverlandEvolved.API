using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Application.Games.Commands;
using NeverlandEvolved.Application.Games.Queries;

namespace NeverlandEvolved.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        // IMediator är navet i CQRS-mönstret. Controllern skickar
        // ett kommando eller en query, och MediatR hittar rätt handler.
        private readonly IMediator _mediator;

        public GamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Games
        // Hämtar alla spel och returnerar dem som en lista av GameDTOs.
        // Öppen för alla — kräver ingen autentisering.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            var games = await _mediator.Send(new GetAllGamesQuery());
            return Ok(games);
        }

        // GET: api/Games/5
        // Hämtar ett specifikt spel via dess ID.
        // Returnerar 404 NotFound om spelet inte finns.
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var gameDto = await _mediator.Send(new GetGameByIdQuery(id));
            if (gameDto == null) return NotFound();

            return Ok(gameDto);
        }

        // POST: api/Games
        // Skapar ett nytt spel. ValidationBehavior kontrollerar datan automatiskt.
        // Returnerar 201 Created med den skapade resursen och dess URL.
        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateGame(CreateGameCommand command)
        {
            var createdGameDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetGame), new { id = createdGameDto.Id }, createdGameDto);
        }

        // PUT: api/Games/5
        // Uppdaterar ett befintligt spel. Kräver Admin-rollen.
        // ID:t i URL:en måste matcha ID:t i request-bodyn.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGame(int id, UpdateGameCommand command)
        {
            if (id != command.Id) return BadRequest();

            var success = await _mediator.Send(command);
            if (!success) return NotFound();

            return NoContent(); // 204 — lyckades men inget att returnera
        }

        // DELETE: api/Games/5
        // Tar bort ett spel permanent. Kräver Admin-rollen.
        // Returnerar 404 om spelet inte finns.
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var success = await _mediator.Send(new DeleteGameCommand(id));
            if (!success) return NotFound();

            return NoContent(); // 204 — lyckades men inget att returnera
        }
    }
}
