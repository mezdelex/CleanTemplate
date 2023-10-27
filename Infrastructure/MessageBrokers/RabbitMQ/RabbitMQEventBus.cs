using Application.Abstractions;

namespace Infrastructure.MessageBrokers.RabbitMQ;

internal sealed class RabbitMQEventBus : IEventBus
{
    // TODO: Continue here

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
