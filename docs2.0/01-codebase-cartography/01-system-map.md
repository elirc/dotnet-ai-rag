# System Map

## Backend

The backend uses a layered shape. [Program](../../src/Api/Program.cs#L15-L137) is the composition root. Controllers such as [TicketsController](../../src/Api/Controllers/TicketsController.cs#L8-L55), [IncidentsController](../../src/Api/Controllers/IncidentsController.cs#L8-L47), [LogsController](../../src/Api/Controllers/LogsController.cs#L8-L17), and [DashboardController](../../src/Api/Controllers/DashboardController.cs#L7-L17) expose HTTP endpoints and delegate behavior.

The application layer defines stable contracts. [OperationsDtos](../../src/Application/Operations/OperationsDtos.cs#L5-L115) defines the records the API returns and accepts. [RequestContext](../../src/Application/Common/RequestContext.cs#L5-L18) turns claims into role capabilities so services do not read HTTP primitives directly.

Infrastructure owns EF Core, auth implementations, dashboard queries, operations behavior, seeding, and the background outbox loop. [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L13-L300) maps tables and relationships. [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L14-L569) implements the current business workflows. [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L9-L52) processes pending outbox rows.

## Frontend

The frontend is a routed Vite React app. [App](../../frontend/src/app/App.tsx#L16-L43) declares the route map. [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73) defines the role-aware shell. [api.ts](../../frontend/src/app/api.ts#L24-L91) centralizes HTTP calls. Pages such as [TicketsPage](../../frontend/src/pages/TicketsPage.tsx#L6-L29), [IncidentsPage](../../frontend/src/pages/IncidentsPage.tsx#L6-L43), [LogsPage](../../frontend/src/pages/LogsPage.tsx#L6-L29), [AgentsPage](../../frontend/src/pages/AgentsPage.tsx#L6-L39), and [EvalsPage](../../frontend/src/pages/EvalsPage.tsx#L6-L29) use React Query for data loading.

## Operational Boundary

This is a training app, but it now has production-like concerns: rate-limited login in [Program](../../src/Api/Program.cs#L19-L31), authorization policies in [Program](../../src/Api/Program.cs#L69-L74), global problem responses in [ExceptionHandlingMiddleware](../../src/Api/Middleware/ExceptionHandlingMiddleware.cs#L6-L43), correlation/security headers in [CorrelationAndSecurityHeadersMiddleware](../../src/Api/Middleware/CorrelationAndSecurityHeadersMiddleware.cs#L3-L23), and CI in [ci.yml](../../.github/workflows/ci.yml#L1-L30).
