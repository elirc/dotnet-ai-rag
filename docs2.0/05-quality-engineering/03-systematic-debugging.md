# Systematic Debugging

## Scenario: Tickets Page Shows Nothing

Trace:

1. Does [TicketsPage](../../frontend/src/pages/TicketsPage.tsx#L6-L17) show loading, error, or empty data?
2. Does [api.ts](../../frontend/src/app/api.ts#L24-L41) send an authorization header?
3. Does [TicketsController.List](../../src/Api/Controllers/TicketsController.cs#L13-L16) receive the request?
4. Does [OperationsService.ListTicketsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L16-L58) filter rows by role?
5. Does [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L128-L134) create seed tickets?

## Scenario: Run Agent Button Is Missing

Check role gating in [IncidentsPage](../../frontend/src/pages/IncidentsPage.tsx#L19-L36). Then verify role claims are parsed in [RequestContext](../../src/Application/Common/RequestContext.cs#L5-L18) and policies are registered in [Program](../../src/Api/Program.cs#L69-L74).

Debugging habit: write down the expected boundary between UI state, HTTP state, service authorization, and database state before changing code.
