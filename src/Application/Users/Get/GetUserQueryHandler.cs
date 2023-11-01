using Application.Contexts;
using Application.Users.Shared;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Get;

public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDTO>
{
    private readonly IApplicationDbContext _context;

    public GetUserQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken)
            ?? throw new UserNotFoundException(request.Id);

        return new UserDTO(user.Id, user.Name, user.Email);
    }
}
