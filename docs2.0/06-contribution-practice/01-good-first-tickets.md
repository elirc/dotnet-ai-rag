# Good First Tickets

## Ticket 1: Add Empty States To Agents Lists

Context: [AgentsPage](../../frontend/src/pages/AgentsPage.tsx#L15-L37) maps runs and approvals but does not show special empty copy.

Acceptance criteria:

- Show a useful empty state when there are no agent runs.
- Show a useful empty state when there are no approvals.
- Keep loading and error behavior intact.
- Run `npm run build`.

## Ticket 2: Add Negative Role Tests

Context: [RequestContextTests](../../tests/UnitTests/RequestContextTests.cs#L9-L29) tests capability mapping.

Acceptance criteria:

- Add explicit tests for Viewer mutation denial.
- Cover Admin, Engineer, SupportAnalyst, Viewer expectations.
- Run `dotnet test NetAiOps.sln`.

## Ticket 3: Bound Log Query Text

Context: [OperationsService.SearchLogsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L439-L466) accepts `query`.

Acceptance criteria:

- Reject or clamp unreasonably long query text.
- Add a test or document why validation is handled elsewhere.
- Preserve existing service/level filters.
