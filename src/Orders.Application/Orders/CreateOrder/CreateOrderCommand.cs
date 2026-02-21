using MediatR;

namespace Orders.Application.Orders.CreateOrder;

public sealed record CreateOrderCommand(string CustomerEmail, decimal Total) : IRequest<Guid>;