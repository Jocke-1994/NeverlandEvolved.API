using MediatR;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Games.Queries
{
    public class GetGameByIdQuery : IRequest<Game?>
    {
        public int Id { get; set; }
        public GetGameByIdQuery(int id) => Id = id;
    }

    public class GetGameByIdQueryHandler : IRequestHandler<GetGameByIdQuery, Game?>
    {
        private readonly IGameRepository _repository;
        public GetGameByIdQueryHandler(IGameRepository repository) => _repository = repository;

        public async Task<Game?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}