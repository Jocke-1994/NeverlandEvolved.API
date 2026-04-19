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
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Hämta alla användare
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        // Hämta en specifik användare via ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Skapa en ny användare
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserCommand command)
        {
            var userDto = await _mediator.Send(command);

            // Returnerar en 201 Created med en länk till den nya användaren
            return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto);
        }
    }
}