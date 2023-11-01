using Application.Users.Shared;
using MediatR;

namespace Application.Users.Get;

public record GetUserQuery(Guid Id) : IRequest<UserDTO>;
