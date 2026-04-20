using MediatR;
using Microsoft.AspNetCore.Mvc;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Application.Reviews.Commands;

namespace NeverlandEvolved.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        // IMediator används som "mellanhand" — controllern skickar ett kommando
        // och MediatR hittar rätt handler som utför arbetet.
        // Controllern vet ingenting om databas, EF Core eller repositories.
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Reviews
        // Skapar en ny recension kopplad till ett spel och en användare.
        // Returnerar 201 Created med den skapade recensionen som ReviewDto.
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewCommand command)
        {
            // MediatR skickar kommandot vidare till CreateReviewCommandHandler.
            // ValidationBehavior körs automatiskt INNAN handlern anropas.
            var reviewDto = await _mediator.Send(command);

            return Ok(reviewDto);
        }
    }
}
