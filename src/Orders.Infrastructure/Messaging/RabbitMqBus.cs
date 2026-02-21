using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Orders.Infrastructure.Messaging;

public sealed class RabbitMqBus : IMessageBus, IDisposable
{
    private readonly IConnection _conn;
    private readonly IModel _ch;

    public RabbitMqBus(string hostName)
    {
        var factory = new ConnectionFactory { HostName = hostName };
        _conn = factory.CreateConnection();
        _ch = _conn.CreateModel();
    }

    public Task PublishAsync<T>(T message, string exchange, CancellationToken ct)
    {
        _ch.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout, durable: true);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _ch.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: body);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _ch.Dispose();
        _conn.Dispose();
    }
}