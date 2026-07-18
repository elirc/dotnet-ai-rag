# Key Flows

## Ticket List

Frontend [TicketsPage](../../frontend/src/pages/TicketsPage.tsx#L6-L29) calls [getTickets](../../frontend/src/app/api.ts#L53-L56). The API endpoint is [TicketsController.List](../../src/Api/Controllers/TicketsController.cs#L13-L16). The service clamps pagination, filters by readable resource scope, counts total rows, and maps DTOs in [OperationsService.ListTicketsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L16-L58). Ticket tables and relationships are mapped in [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L53-L108).

## Ticket Mutation

[TicketsController](../../src/Api/Controllers/TicketsController.cs#L26-L55) exposes create, status update, assignment, and comment endpoints. [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L76-L191) guards support mutations, validates status transitions, adds audit events, and writes outbox messages.

## Incident Investigation

[IncidentsPage](../../frontend/src/pages/IncidentsPage.tsx#L19-L36) shows the "Run agent" action only to Admin and Engineer roles. The client calls [runIncidentInvestigation](../../frontend/src/app/api.ts#L63-L64), then [IncidentsController.RunInvestigation](../../src/Api/Controllers/IncidentsController.cs#L42-L47). [OperationsService.RunIncidentInvestigationAsync](../../src/Infrastructure/Operations/OperationsService.cs#L303-L360) writes timeline, agent, approval, audit, and outbox records in one save.

## Dashboard Metrics

[DashboardPage](../../frontend/src/pages/DashboardPage.tsx#L21-L65) renders summary cards and recent recommendations. [DashboardController.Summary](../../src/Api/Controllers/DashboardController.cs#L12-L16) calls [DashboardService.GetSummaryAsync](../../src/Infrastructure/Persistence/DashboardService.cs#L8-L53), which calculates counts from incidents, tickets, logs, outbox messages, eval runs, and approvals.

## Log Search

[LogsPage](../../frontend/src/pages/LogsPage.tsx#L6-L29) loads synthetic logs through [getLogs](../../frontend/src/app/api.ts#L65-L66). [LogsController.Search](../../src/Api/Controllers/LogsController.cs#L13-L17) passes optional filters into [OperationsService.SearchLogsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L363-L390). Log indexes and storage fields are mapped in [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L139-L173).

## Role-Aware Navigation

[AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73) hides navigation items based on role. Server-side authorization still matters: [Program](../../src/Api/Program.cs#L69-L74) defines policies, and mutation guards in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L441-L467) enforce capabilities even if a user bypasses the UI.

## Outbox Processing

Operations write outbox messages through [AddOutboxMessage](../../src/Infrastructure/Operations/OperationsService.cs#L487-L493). [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L9-L52) periodically reads pending messages and marks them processed. The current worker is deliberately simple; production follow-up work should add retry policy, poison-message handling, and observable delivery outcomes.
