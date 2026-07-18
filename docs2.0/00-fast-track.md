# 00 Fast Track

## What You Are Looking At

NetAiOps is now more than a foundation app. The fast mental model is:

1. ASP.NET Core composes auth, policies, middleware, controllers, health checks, and startup seeding in [Program](../src/Api/Program.cs#L15-L137).
2. Controllers stay thin and delegate to application-facing services, as shown by [TicketsController](../src/Api/Controllers/TicketsController.cs#L8-L55) and [IncidentsController](../src/Api/Controllers/IncidentsController.cs#L8-L47).
3. Most operational behavior lives in [OperationsService](../src/Infrastructure/Operations/OperationsService.cs#L12-L493).
4. EF Core maps the operational model in [NetAiOpsDbContext](../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L13-L300).
5. React Router exposes the product surface in [App](../frontend/src/app/App.tsx#L16-L43), and each page uses TanStack Query through [api.ts](../frontend/src/app/api.ts#L43-L91).

## First Ten Files

Read these in order:

1. [README](../README.md#L1-L79) for the product promise and setup.
2. [Program](../src/Api/Program.cs#L15-L137) for runtime composition.
3. [OperationsDtos](../src/Application/Operations/OperationsDtos.cs#L5-L115) for API contracts.
4. [OperationsService](../src/Infrastructure/Operations/OperationsService.cs#L12-L493) for behavior.
5. [NetAiOpsDbContext](../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L13-L300) for persistence.
6. [DatabaseSeeder](../src/Infrastructure/Persistence/DatabaseSeeder.cs#L21-L248) for realistic scenarios.
7. [App](../frontend/src/app/App.tsx#L16-L43) for frontend routes.
8. [AppLayout](../frontend/src/components/AppLayout.tsx#L5-L73) for navigation and role filtering.
9. [api.ts](../frontend/src/app/api.ts#L24-L91) for client/server contracts.
10. [RequestContextTests](../tests/UnitTests/RequestContextTests.cs#L7-L30) for a small example of testing role logic.

## Two Flows To Trace Today

Ticket list: [TicketsPage](../frontend/src/pages/TicketsPage.tsx#L6-L29) calls [getTickets](../frontend/src/app/api.ts#L53-L56), which reaches [TicketsController.List](../src/Api/Controllers/TicketsController.cs#L13-L16), which calls [OperationsService.ListTicketsAsync](../src/Infrastructure/Operations/OperationsService.cs#L16-L58), which queries the ticket mapping in [NetAiOpsDbContext](../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L53-L81).

Incident investigation: [IncidentsPage](../frontend/src/pages/IncidentsPage.tsx#L6-L43) calls [runIncidentInvestigation](../frontend/src/app/api.ts#L63-L64), which reaches [IncidentsController.RunInvestigation](../src/Api/Controllers/IncidentsController.cs#L42-L47), which creates an agent run, timeline event, human approval, audit event, and outbox message in [OperationsService.RunIncidentInvestigationAsync](../src/Infrastructure/Operations/OperationsService.cs#L303-L360).

## Verified Commands

Run these before changing code:

```powershell
& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln
cd frontend
npm run build
```

During this documentation pass, both commands passed. `docker compose config` also validated the default Postgres service described in [docker-compose.yml](../docker-compose.yml#L1-L47).

## One-Hour Exercise

Explain, in your own words, why a viewer can read some work but cannot mutate support work. Use [RequestContext](../src/Application/Common/RequestContext.cs#L5-L18), [RequestContextTests](../tests/UnitTests/RequestContextTests.cs#L9-L29), and the mutation guard in [OperationsService](../src/Infrastructure/Operations/OperationsService.cs#L441-L467).
