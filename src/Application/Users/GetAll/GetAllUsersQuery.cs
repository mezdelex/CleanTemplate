using MediatR;

namespace Application.Users.GetAll;

public record UserDTO(Guid Id, string Name, string Email);

public record GetAllUsersQuery() : IRequest<List<UserDTO>>;
