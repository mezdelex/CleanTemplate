using MediatR;

namespace Application.Users.GetAll;

internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _context.Users.Select(user => new UserDTO(user.Id, user.Name, user.Email)).ToList(), cancellationToken);
    }
}
