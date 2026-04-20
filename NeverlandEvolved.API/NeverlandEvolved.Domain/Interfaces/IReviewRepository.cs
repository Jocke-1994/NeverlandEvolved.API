using NeverlandEvolved.Domain.Entities;

namespace NeverlandEvolved.Domain.Interfaces
{
    // IReviewRepository definierar kontraktet för databasoperationer på Review-entiteten.
    // Genom att placera interfacet i Domain-lagret och implementationen i Infrastructure
    // följer vi Dependency Inversion-principen — Application och Domain känner inte till
    // EF Core eller SQL Server, bara till detta kontrakt.
    public interface IReviewRepository
    {
        // Sparar en ny recension och returnerar den sparade versionen (med genererat ID)
        Task<Review> AddAsync(Review review);

        // Hjälpmetoder för att validera kopplingar innan en recension skapas
        Task<bool> GameExistsAsync(int gameId);
        Task<bool> UserExistsAsync(int userId);
    }
}
