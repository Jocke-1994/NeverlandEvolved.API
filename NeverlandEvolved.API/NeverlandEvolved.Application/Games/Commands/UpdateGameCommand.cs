using MediatR;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Games.Commands
{
    public class UpdateGameCommand : IRequest<bool> // Returnerar true om det lyckades
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, bool>
    {
        private readonly IGameRepository _repository;
        public UpdateGameCommandHandler(IGameRepository repository) => _repository = repository;

        public async Task<bool> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            var game = await _repository.GetByIdAsync(request.Id);
            if (game == null) return false;

            game.Title = request.Title;
            game.Genre = request.Genre;
            game.Price = request.Price;

            await _repository.UpdateAsync(game);
            return true;
        }
    }
}