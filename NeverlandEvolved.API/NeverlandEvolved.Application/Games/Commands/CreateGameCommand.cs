using AutoMapper;
using MediatR;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Games.Commands
{
    // Kommandot returnerar nu en GameDto istället för Game
    public class CreateGameCommand : IRequest<GameDto>
    {
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameDto>
    {
        private readonly IGameRepository _repository;
        private readonly IMapper _mapper;

        // Vi injicerar både vårt Repository och vår Mapper
        public CreateGameCommandHandler(IGameRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GameDto> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            // 1. Skapa entiteten från kommandot (här kan man också använda mapper om man vill)
            var newGame = new Game
            {
                Title = request.Title,
                Genre = request.Genre,
                Price = request.Price
            };

            // 2. Spara i databasen via repot
            var savedGame = await _repository.AddAsync(newGame);

            // 3. Mappa om den sparade entiteten (som har fått ett Id) till en DTO
            return _mapper.Map<GameDto>(savedGame);
        }
    }
}