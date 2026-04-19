using AutoMapper;
using MediatR;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Domain.Interfaces;

public class GetUserByIdQuery : IRequest<UserDto?>
{
    public int Id { get; set; }
    public GetUserByIdQuery(int id) => Id = id;
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
}