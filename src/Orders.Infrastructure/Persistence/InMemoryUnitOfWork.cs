using MediatR;
using Orders.Application.Abstractions;
using Orders.Domain.Common;
using Orders.Infrastructure.Persistence;

namespace Orders.Infrastructure.Persistence;

public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly InMemoryOrderRepository _repo;
    private readonly IPublisher _publisher;

    public InMemoryUnitOfWork(InMemoryOrderRepository repo, IPublisher publisher)
    {
        _repo = repo;
        _publisher = publisher;
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        // Simulate "commit", then dispatch domain events from tracked entities
        var domainEntities = _repo.Orders.Cast<Entity>().Where(e => e.DomainEvents.Any()).ToList();
        var events = domainEntities.SelectMany(e => e.DomainEvents).ToList();

        foreach (var entity in domainEntities) entity.ClearDomainEvents();
        foreach (var ev in events) await _publisher.Publish(ev, ct);
    }
}