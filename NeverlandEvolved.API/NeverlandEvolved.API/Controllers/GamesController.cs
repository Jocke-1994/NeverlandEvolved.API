using MediatR;
using Microsoft.AspNetCore.Mvc;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Application.Games.Commands;
using NeverlandEvolved.Application.Games.Queries;
using NeverlandEvolved.Domain.Entities;

namespace NeverlandEvolved.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames() // Ändra till GameDto här
        {
            var games = await _mediator.Send(new GetAllGamesQuery());
            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var gameDto = await _mediator.Send(new GetGameByIdQuery(id));
            if (gameDto == null) return NotFound();

            return Ok(gameDto);
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateGame(CreateGameCommand command)
        {
            var createdGameDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetGame), new { id = createdGameDto.Id }, createdGameDto);
        }
        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, UpdateGameCommand command)
        {
            if (id != command.Id) return BadRequest();

            var success = await _mediator.Send(command);
            if (!success) return NotFound();

            return NoContent();
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var success = await _mediator.Send(new DeleteGameCommand(id));
            if (!success) return NotFound();

            return NoContent();
        }
    }
}