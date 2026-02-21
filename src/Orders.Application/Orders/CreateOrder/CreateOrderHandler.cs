using MediatR;
using Orders.Application.Abstractions;
using Orders.Domain.Orders;

namespace Orders.Application.Orders.CreateOrder;

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateOrderHandler(IOrderRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var order = Order.Create(request.CustomerEmail, request.Total);

        await _repo.AddAsync(order, ct);
        await _uow.SaveChangesAsync(ct);

        return order.Id;
    }
}