using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Users.Create;

public sealed class CreatedUserEventConsumer : IConsumer<CreatedUserEvent>
{
    private readonly ILogger<CreatedUserEventConsumer> _logger;

    public CreatedUserEventConsumer(ILogger<CreatedUserEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CreatedUserEvent> context)
    {
        _logger.LogInformation("User created: {@User}", context.Message);

        return Task.CompletedTask;
    }
}
