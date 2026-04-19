using MediatR;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Games.Commands
{
    public class DeleteGameCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteGameCommand(int id) => Id = id;
    }

    public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, bool>
    {
        private readonly IGameRepository _repository;
        public DeleteGameCommandHandler(IGameRepository repository) => _repository = repository;

        public async Task<bool> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
        {
            var game = await _repository.GetByIdAsync(request.Id);
            if (game == null) return false;

            await _repository.DeleteAsync(game);
            return true;
        }
    }
}