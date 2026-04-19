using MediatR;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Games.Commands
{
    // Kommandot: Innehåller den data vi behöver för att skapa spelet
    public class CreateGameCommand : IRequest<Game>
    {
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    // Handlern: Utför själva jobbet
    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Game>
    {
        private readonly IGameRepository _repository;

        public CreateGameCommandHandler(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<Game> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var newGame = new Game
            {
                Title = request.Title,
                Genre = request.Genre,
                Price = request.Price
            };

            return await _repository.AddAsync(newGame);
        }
    }
}