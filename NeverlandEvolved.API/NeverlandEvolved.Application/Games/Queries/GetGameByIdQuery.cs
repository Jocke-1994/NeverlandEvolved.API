using MediatR;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;
using AutoMapper;
using NeverlandEvolved.Application.DTOs; // Glöm inte denna

namespace NeverlandEvolved.Application.Games.Queries
{
    public class GetGameByIdQuery : IRequest<GameDto?>
    {
        public int Id { get; set; }
        public GetGameByIdQuery(int id) => Id = id;
    }

    public class GetGameByIdQueryHandler : IRequestHandler<GetGameByIdQuery, GameDto?>
    {
        private readonly IGameRepository _repository;
        private readonly IMapper _mapper;
        public GetGameByIdQueryHandler(IGameRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GameDto?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await _repository.GetByIdAsync(request.Id);

        
            return game == null ? null : _mapper.Map<GameDto>(game);
        }
    }
}