using AutoMapper;
using FluentValidation;
using MediatR;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Reviews.Commands
{
    // Kommandot innehåller all data som behövs för att skapa en recension.
    // Det implementerar IRequest<ReviewDto> vilket berättar för MediatR
    // att detta kommando ska returnera en ReviewDto när det hanteras.
    public class CreateReviewCommand : IRequest<ReviewDto>
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int GameId { get; set; }
        public int UserId { get; set; }
    }

    // Validator körs automatiskt via ValidationBehavior i MediatR-pipelinen
    // INNAN handlern anropas. Om reglerna inte uppfylls kastas ett undantag
    // som fångas upp av ExceptionHandlingMiddleware och returneras som 400.
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Betyget måste vara mellan 1 och 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Kommentaren får inte vara tom.")
                .MaximumLength(500).WithMessage("Kommentaren får vara max 500 tecken.");

            RuleFor(x => x.GameId)
                .GreaterThan(0).WithMessage("Ett giltigt spel-ID måste anges.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Ett giltigt användar-ID måste anges.");
        }
    }

    // Handlern är den klass som faktiskt utför arbetet när kommandot skickas.
    // Den tar emot kommandot från MediatR, sparar recensionen i databasen
    // och returnerar en DTO tillbaka till controllern.
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // Kontrollera att spelet och användaren faktiskt finns
            bool gameExists = await _repository.GameExistsAsync(request.GameId);
            if (!gameExists)
                throw new InvalidOperationException($"Spelet med ID {request.GameId} hittades inte.");

            bool userExists = await _repository.UserExistsAsync(request.UserId);
            if (!userExists)
                throw new InvalidOperationException($"Användaren med ID {request.UserId} hittades inte.");

            // Bygg upp entiteten från kommando-datan
            var review = new Review
            {
                Rating = request.Rating,
                Comment = request.Comment,
                GameId = request.GameId,
                UserId = request.UserId
            };

            // Spara i databasen och mappa resultatet till en DTO
            var savedReview = await _repository.AddAsync(review);
            return _mapper.Map<ReviewDto>(savedReview);
        }
    }
}
