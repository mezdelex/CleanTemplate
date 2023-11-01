using Application.Contexts;
using Application.Users.Shared;
using MediatR;
using static Domain.Extensions.Collections.Collections;

namespace Application.Users.GetAll;

public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedList<UserDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken) =>
         await _context.Users
             .ToDynamicOrder(request.DynamicOrderParams)
             .Select(user => new UserDTO(user.Id, user.Name, user.Email))
             .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);
}
