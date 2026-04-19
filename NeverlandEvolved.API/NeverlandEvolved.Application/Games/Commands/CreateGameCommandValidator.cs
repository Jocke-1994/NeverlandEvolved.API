using FluentValidation;

namespace NeverlandEvolved.Application.Games.Commands
{
    public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
    {
        public CreateGameCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Titeln får inte vara tom.")
                .MaximumLength(100).WithMessage("Titeln får vara max 100 tecken.");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Du måste ange en genre.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Priset måste vara högre än 0 kr.");
        }
    }
}