using Orders.Domain.Common;

namespace Orders.Domain.Orders;

public sealed class Order : Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string CustomerEmail { get; private set; } = default!;
    public decimal Total { get; private set; }

    private Order() { }

    public static Order Create(string email, decimal total)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.");
        if (total <= 0) throw new ArgumentException("Total must be positive.");

        var order = new Order { CustomerEmail = email, Total = total };
        order.Raise(new OrderCreated(order.Id, order.CustomerEmail, order.Total));
        return order;
    }
}

public sealed record OrderCreated(Guid OrderId, string CustomerEmail, decimal Total)
    : DomainEvent(DateTime.UtcNow);