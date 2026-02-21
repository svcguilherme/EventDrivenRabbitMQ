using MediatR;

namespace Orders.Domain.Common;

public abstract record DomainEvent(DateTime OccurredOnUtc) : INotification;