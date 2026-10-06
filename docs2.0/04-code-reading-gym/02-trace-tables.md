# Trace Tables

## Trace: Support Analyst Opens Tickets

| Step | File | What To Record |
| --- | --- | --- |
| 1 | [TicketsPage](../../frontend/src/pages/TicketsPage.tsx#L6-L29) | Query key and loading/error behavior |
| 2 | [api.ts](../../frontend/src/app/api.ts#L53-L56) | URL and auth header path |
| 3 | [TicketsController](../../src/Api/Controllers/TicketsController.cs#L13-L16) | HTTP method and request context creation |
| 4 | [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L18-L60) | Filtering, count, paging |
| 5 | [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L517-L528) | Why only some tickets may appear |

## Trace: Agent Run Appears After Investigation

| Step | File | What To Record |
| --- | --- | --- |
| 1 | [IncidentsPage](../../frontend/src/pages/IncidentsPage.tsx#L11-L14) | Mutation and invalidation target |
| 2 | [api.ts](../../frontend/src/app/api.ts#L63-L64) | Endpoint path |
| 3 | [IncidentsController](../../src/Api/Controllers/IncidentsController.cs#L42-L47) | Controller action |
| 4 | [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L305-L436) | Created records |
| 5 | [AgentsPage](../../frontend/src/pages/AgentsPage.tsx#L6-L39) | Query that makes the new run visible |
