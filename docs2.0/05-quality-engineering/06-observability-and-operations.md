# Observability And Operations

The app includes request correlation through [CorrelationAndSecurityHeadersMiddleware](../../src/Api/Middleware/CorrelationAndSecurityHeadersMiddleware.cs#L3-L23) and health checks through [Program](../../src/Api/Program.cs#L119-L130). Synthetic logs are seeded in [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L309-L340) and surfaced through [LogsPage](../../frontend/src/pages/LogsPage.tsx#L6-L29).

The dashboard gives an operations summary from database counts in [DashboardService](../../src/Infrastructure/Persistence/DashboardService.cs#L8-L53). Agent runs and approval queues are exposed in [AgentsPage](../../frontend/src/pages/AgentsPage.tsx#L6-L39).

Production exercises:

1. Add structured logging around ticket and incident mutations.
2. Add metrics for outbox pending, processed, failed, and retry counts.
3. Add a dashboard card for pending human approvals.
4. Make correlation ID visible in API error responses.
