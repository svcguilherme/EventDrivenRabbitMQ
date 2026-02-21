namespace Contracts;

public sealed record OrderCreatedMessage(Guid OrderId, string CustomerEmail, decimal Total);