# Framework Mental Models

## ASP.NET Core

Controllers translate HTTP into application calls. [LogsController.Search](../../src/Api/Controllers/LogsController.cs#L13-L17) is a good tiny example: it accepts query parameters, builds a [RequestContext](../../src/Application/Common/RequestContext.cs#L5-L18), and delegates to the operations service.

Middleware wraps requests before controllers run. [ExceptionHandlingMiddleware](../../src/Api/Middleware/ExceptionHandlingMiddleware.cs#L6-L43) converts unexpected exceptions to problem responses. [CorrelationAndSecurityHeadersMiddleware](../../src/Api/Middleware/CorrelationAndSecurityHeadersMiddleware.cs#L3-L23) adds request correlation and basic browser-facing headers.

## EF Core

EF Core maps domain objects to relational tables in [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L33-L300). Services build LINQ queries, as in [OperationsService.ListTicketsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L18-L60) and [OperationsService.SearchLogsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L439-L466).

## React Query

The frontend treats server state as cacheable async data. [DashboardPage](../../frontend/src/pages/DashboardPage.tsx#L5-L18), [TicketsPage](../../frontend/src/pages/TicketsPage.tsx#L6-L13), and [AgentsPage](../../frontend/src/pages/AgentsPage.tsx#L6-L13) show loading and error handling close to the view.
