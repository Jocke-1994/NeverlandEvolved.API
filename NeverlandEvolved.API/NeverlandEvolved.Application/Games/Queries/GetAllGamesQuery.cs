using AutoMapper;
using MediatR;
using NeverlandEvolved.Application.DTOs; // Glöm inte denna
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Games.Queries
{
    // 1. Vi vill nu få tillbaka en lista av DTOs istället för entiteter
    public class GetAllGamesQuery : IRequest<IEnumerable<GameDto>>
    {
    }

    public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, IEnumerable<GameDto>>
    {
        private readonly IGameRepository _repository;
        private readonly IMapper _mapper; // 2. Vi lägger till mappen här

        public GetAllGamesQueryHandler(IGameRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameDto>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
        {
            var games = await _repository.GetAllAsync();

            // 3. Här sker magin! Vi mappar listan av Game -> GameDto
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }
    }
}