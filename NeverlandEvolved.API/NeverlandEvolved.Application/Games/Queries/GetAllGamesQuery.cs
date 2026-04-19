using MediatR;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Games.Queries
{
    // 1. Själva frågan (Berättar att vi förväntar oss en lista med spel tillbaka)
    public class GetAllGamesQuery : IRequest<IEnumerable<Game>>
    {
    }

    // 2. Handlern (Den som faktiskt utför jobbet när någon ställer frågan)
    public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, IEnumerable<Game>>
    {
        private readonly IGameRepository _repository;

        public GetAllGamesQueryHandler(IGameRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Game>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}