# Dotnet Runtime Model

ASP.NET Core starts from [Program](../../src/Api/Program.cs#L15-L137). Services are registered first, then middleware and endpoints are composed. Notice how infrastructure services are registered before the app is built in [Program](../../src/Api/Program.cs#L76-L78), and middleware order is explicit in [Program](../../src/Api/Program.cs#L104-L130).

Dependency injection is the normal object graph. Controllers receive service abstractions through constructors, as seen in [TicketsController](../../src/Api/Controllers/TicketsController.cs#L11-L16). Infrastructure registers concrete implementations in [InfrastructureServiceCollectionExtensions](../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L16-L30).

Background work is hosted by the runtime. [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L9-L28) loops until cancellation and creates a dependency-injection scope for each processing pass in [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L32-L52).

Junior exercise: draw the startup sequence from service registration to first `GET /api/tickets` request.
