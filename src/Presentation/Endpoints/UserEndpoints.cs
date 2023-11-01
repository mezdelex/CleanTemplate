using Application.Users.Create;
using Application.Users.Get;
using Application.Users.GetAll;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Presentation.Endpoints;

public static class UserEndpoints
{
    private static readonly ILogger _logger = new LoggerFactory().CreateLogger("User");

    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users/");

        group.MapGet("", GetAllUsersAsync);
        group.MapGet("{id:guid}", GetUserAsync);
        group.MapPost("", CreateUserAsync);
    }

    public static async Task<IResult> GetAllUsersAsync([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? dynamicOrderParams, ISender sender)
    {
        try
        {
            return Results.Ok(await sender.Send(new GetAllUsersQuery(page, pageSize, dynamicOrderParams!)));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);

            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> GetUserAsync([FromRoute] Guid id, ISender sender)
    {
        try
        {
            return Results.Ok(await sender.Send(new GetUserQuery(id)));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);

            return Results.NotFound(e.Message);
        }
    }

    public static async Task<IResult> CreateUserAsync([FromBody] CreateUserCommand command, ISender sender)
    {
        try
        {
            await sender.Send(command);

            return Results.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);

            return Results.BadRequest(e.Message);
        }
    }
}
