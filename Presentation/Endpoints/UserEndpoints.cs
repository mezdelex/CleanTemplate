using Application.Users.Create;
using MediatR;

namespace Presentation.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users/");

        group.MapPost("", CreateUser);
    }

    public static async Task<IResult> CreateUser(CreateUserCommand command, ISender sender)
    {
        await sender.Send(command);

        return Results.Ok();
    }
}
