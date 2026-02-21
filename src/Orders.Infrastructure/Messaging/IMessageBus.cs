namespace Orders.Infrastructure.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, string exchange, CancellationToken ct);
}