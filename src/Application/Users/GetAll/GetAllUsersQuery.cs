using Application.Users.Shared;
using MediatR;
using static Domain.Extensions.Collections.Collections;

namespace Application.Users.GetAll;

public record GetAllUsersQuery(int Page, int PageSize, string DynamicOrderParams = "") : IRequest<PagedList<UserDTO>>;
