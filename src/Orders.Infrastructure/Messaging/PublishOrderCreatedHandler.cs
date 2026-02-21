using Contracts;
using MediatR;
using Orders.Domain.Orders;
using Orders.Infrastructure.Messaging;

public sealed class PublishOrderCreatedHandler : INotificationHandler<OrderCreated>
{
    private readonly IMessageBus _bus;

    public PublishOrderCreatedHandler(IMessageBus bus) => _bus = bus;

    public Task Handle(OrderCreated notification, CancellationToken ct)
    {
        var msg = new OrderCreatedMessage(notification.OrderId, notification.CustomerEmail, notification.Total);
        return _bus.PublishAsync(msg, "orders.order-created", ct);
    }
}