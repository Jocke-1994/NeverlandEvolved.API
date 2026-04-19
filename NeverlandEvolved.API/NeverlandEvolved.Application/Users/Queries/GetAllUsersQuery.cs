using AutoMapper;
using MediatR;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Domain.Interfaces;

namespace NeverlandEvolved.Application.Users.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>> { }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}