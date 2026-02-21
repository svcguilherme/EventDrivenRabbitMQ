# 🐇 Event-Driven Orders System

## .NET 8 • RabbitMQ • Clean Architecture • Minimal APIs

> A production-style event-driven microservice sample demonstrating
> asynchronous communication using RabbitMQ and modern .NET patterns.

------------------------------------------------------------------------

# 🌍 Why This Project Exists

Modern distributed systems must be:

-   Scalable
-   Decoupled
-   Fault-tolerant
-   Asynchronous

This project demonstrates how real enterprise systems handle background
processing using an **event-driven architecture**.

------------------------------------------------------------------------

# 🚀 Running the Application

## Prerequisites

-   .NET 8 SDK
-   Docker
-   RabbitMQ

Run RabbitMQ:

docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p
15672:15672 rabbitmq:3-management

Dashboard: http://localhost:15672

User: guest / guest

------------------------------------------------------------------------

## Run API

cd Orders.Api dotnet run

------------------------------------------------------------------------

## Run Consumer

cd Orders.Consumer dotnet run

------------------------------------------------------------------------

# 🧪 Test

curl -X POST http://localhost:5000/orders -H "Content-Type:
application/json" -d
'{"customerEmail":"demo@example.com","total":99.90}'

------------------------------------------------------------------------

# 👨‍💻 Author

Guilherme Sant'Anna
