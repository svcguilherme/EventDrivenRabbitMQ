using Orders.Domain.Orders;

namespace Orders.Application.Abstractions;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct);
}