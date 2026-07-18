# Annotation Drills

## Drill 1: Ticket Pagination

Annotate [OperationsService.ListTicketsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L16-L58). Mark where it clamps inputs, applies resource authorization, counts rows, orders rows, pages rows, and maps DTOs.

Self-grade:

- Beginner: can explain what the method returns.
- Intermediate: can explain why count happens before paging.
- Strong: can identify how `CanReadTicket` affects SupportAnalyst visibility.

## Drill 2: Incident Investigation

Annotate [OperationsService.RunIncidentInvestigationAsync](../../src/Infrastructure/Operations/OperationsService.cs#L303-L360). Mark each row or entity written. Explain why the method creates a human approval instead of closing the incident automatically.

## Drill 3: Frontend Mutation

Annotate [IncidentsPage](../../frontend/src/pages/IncidentsPage.tsx#L6-L43). Mark query, mutation, cache invalidation, role gate, loading state, error state, and action button.
