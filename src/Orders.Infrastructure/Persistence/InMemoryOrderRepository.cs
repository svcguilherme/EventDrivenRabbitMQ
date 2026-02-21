using Orders.Application.Abstractions;
using Orders.Domain.Orders;

namespace Orders.Infrastructure.Persistence;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();
    public IReadOnlyList<Order> Orders => _orders;

    public Task AddAsync(Order order, CancellationToken ct)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }
}