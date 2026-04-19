using AutoMapper;
using FluentValidation;
using MediatR;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Domain.Entities;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _repository; // Se till att du har ett IUserRepository!
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User { Username = request.Username, Email = request.Email, Password = request.Password };
            var savedUser = await _repository.AddAsync(user);
            return _mapper.Map<UserDto>(savedUser);
        }
    }
}