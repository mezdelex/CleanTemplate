using MediatR;

namespace Application.Users.Get;

public record UserDTO(Guid Id, string Name, string Email);

public record GetUserQuery(Guid Id) : IRequest<UserDTO>;
