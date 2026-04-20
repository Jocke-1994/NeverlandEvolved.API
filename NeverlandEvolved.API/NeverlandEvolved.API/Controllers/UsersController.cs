using MediatR;
using Microsoft.AspNetCore.Mvc;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Application.Users.Commands;
using NeverlandEvolved.Application.Users.Queries;

namespace NeverlandEvolved.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // IMediator kopplar controllern till Application-lagret utan direkt beroende.
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Users
        // Hämtar alla registrerade användare som UserDTOs.
        // Lösenord ingår INTE i UserDto — de exponeras aldrig mot klienten.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        // GET: api/Users/5
        // Hämtar en specifik användare via ID.
        // Returnerar 404 NotFound om användaren inte finns.
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/Users
        // Skapar en ny användare. FluentValidation kontrollerar att
        // användarnamn, e-post och lösenord uppfyller reglerna.
        // Returnerar 201 Created med en länk till den nya användaren.
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserCommand command)
        {
            var userDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto);
        }
    }
}
