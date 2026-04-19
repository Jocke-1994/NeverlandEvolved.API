using Microsoft.AspNetCore.Mvc;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Infrastructure.Data;

namespace NeverlandEvolved.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Reviews (Skapar en recension kopplad till spel och användare)
        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            // Säkerhetskoll: Finns spelet?
            var gameExists = await _context.Games.FindAsync(review.GameId);
            if (gameExists == null) return BadRequest("Spelet hittades inte i databasen.");

            // Säkerhetskoll: Finns användaren?
            var userExists = await _context.Users.FindAsync(review.UserId);
            if (userExists == null) return BadRequest("Användaren hittades inte i databasen.");

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }
    }
}