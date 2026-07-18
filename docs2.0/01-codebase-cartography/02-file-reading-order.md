# File Reading Order

## Backend First Pass

1. [Program](../../src/Api/Program.cs#L15-L137): learn composition, auth, middleware, and startup seeding.
2. [OperationsDtos](../../src/Application/Operations/OperationsDtos.cs#L5-L115): learn the API vocabulary before reading implementation.
3. [RequestContext](../../src/Application/Common/RequestContext.cs#L5-L18): learn role capability flags.
4. [TicketsController](../../src/Api/Controllers/TicketsController.cs#L8-L55) and [IncidentsController](../../src/Api/Controllers/IncidentsController.cs#L8-L47): learn the HTTP surface.
5. [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L12-L493): learn business behavior.
6. [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L13-L300): learn storage shape.
7. [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L21-L248): learn the demo scenarios.
8. [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L9-L52): learn asynchronous side effects.

## Frontend First Pass

1. [App](../../frontend/src/app/App.tsx#L16-L43): routes and protected layout.
2. [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73): navigation, role filtering, logout.
3. [api.ts](../../frontend/src/app/api.ts#L24-L91): request wrapper and endpoint map.
4. [DashboardPage](../../frontend/src/pages/DashboardPage.tsx#L5-L68): database-backed summary display.
5. [TicketsPage](../../frontend/src/pages/TicketsPage.tsx#L6-L29): list workflow.
6. [IncidentsPage](../../frontend/src/pages/IncidentsPage.tsx#L6-L43): mutation workflow.
7. [AgentsPage](../../frontend/src/pages/AgentsPage.tsx#L6-L39): human-in-the-loop visibility.

## Why This Order Works

You learn contracts before internals, then flows before edge cases. That keeps you from drowning in EF mappings before you know the business verbs.
