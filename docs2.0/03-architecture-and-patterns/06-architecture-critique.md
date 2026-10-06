# Architecture Critique

## Strengths

The repo now has a credible training surface: real operational entities, database-backed dashboard metrics, bounded list APIs, resource-aware checks, seeded incidents/logs/runbooks, human approvals, an outbox worker, middleware, CI, and multiple frontend workspaces. These are visible across [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L14-L569), [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L13-L300), [Program](../../src/Api/Program.cs#L15-L137), and [App](../../frontend/src/app/App.tsx#L16-L43).

## Risks

[OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L14-L569) is doing too much. That is acceptable for a training phase, but it will become painful as tests and workflows expand.

Validation is light. Controllers accept request DTOs, but there is no comprehensive validation layer anchored in the current code. That is a production gap.

The outbox worker has no retries or failure model beyond simple processing in [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L32-L52).

Startup migrations in [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L16-L18) are convenient but risky for real production.

Frontend pages are useful but mostly read-oriented. [IncidentsPage](../../frontend/src/pages/IncidentsPage.tsx#L11-L14) has one mutation; tickets and settings do not yet expose full create/edit workflows.

## Senior Recommendation

The next production-readiness sprint should add integration tests around API authorization and persistence, split `OperationsService` by workflow ownership, harden validation, and make outbox processing observable.
