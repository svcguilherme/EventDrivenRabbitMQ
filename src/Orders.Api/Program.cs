using FluentValidation;
using MediatR;
using Orders.Application;
using Orders.Application.Behaviors;
using Orders.Application.Orders.CreateOrder;
using Orders.Infrastructure.Messaging;
using Orders.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// MediatR + validators
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyRef).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyRef).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Infrastructure registrations
builder.Services.AddSingleton<IMessageBus>(_ =>
    new RabbitMqBus(hostName: builder.Configuration["RabbitMq:Host"] ?? "localhost"));

// In-memory persistence
builder.Services.AddSingleton<InMemoryOrderRepository>();
builder.Services.AddScoped<Orders.Application.Abstractions.IOrderRepository>(sp => sp.GetRequiredService<InMemoryOrderRepository>());
builder.Services.AddScoped<Orders.Application.Abstractions.IUnitOfWork, InMemoryUnitOfWork>();

var app = builder.Build();

// FluentValidation exception mapping
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (ValidationException ex)
    {
        ctx.Response.StatusCode = 400;
        await ctx.Response.WriteAsJsonAsync(new
        {
            error = "Validation failed",
            details = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
        });
    }
});

// Endpoint
app.MapPost("/orders", async (CreateOrderRequest req, IMediator mediator, CancellationToken ct) =>
{
    var id = await mediator.Send(new CreateOrderCommand(req.CustomerEmail, req.Total), ct);
    return Results.Created($"/orders/{id}", new { id });
});

app.Run();

public sealed record CreateOrderRequest(string CustomerEmail, decimal Total);