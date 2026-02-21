using System.Text;
using System.Text.Json;
using Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var host = "localhost";
var exchange = "orders.order-created";

var factory = new ConnectionFactory { HostName = host };
using var conn = factory.CreateConnection();
using var ch = conn.CreateModel();

ch.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout, durable: true);

// Create an exclusive queue and bind to the fanout exchange
var queueName = ch.QueueDeclare().QueueName;
ch.QueueBind(queue: queueName, exchange: exchange, routingKey: "");

Console.WriteLine($"[RabbitMQ] Listening on exchange '{exchange}'...");

var consumer = new EventingBasicConsumer(ch);
consumer.Received += (_, ea) =>
{
    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
    var msg = JsonSerializer.Deserialize<OrderCreatedMessage>(json);

    Console.WriteLine($"[RabbitMQ] OrderCreated: {msg?.OrderId} | {msg?.CustomerEmail} | {msg?.Total}");
};

ch.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

Console.WriteLine("Press ENTER to exit.");
Console.ReadLine();