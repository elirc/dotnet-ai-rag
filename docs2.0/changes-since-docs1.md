# Changes Since Docs 1

## Why The Old Docs Are Out Of Date

The first documentation set accurately described a Phase 1 foundation, but the codebase has grown beyond that snapshot. The old architecture doc says ticket and incident persistence were deliberately avoided and that the dashboard used placeholder metrics in [architecture.md](../docs/architecture.md#L37-L39). That is no longer true: tickets, incidents, logs, knowledge documents, agent runs, eval runs, admin settings, approvals, audit events, and outbox messages are now mapped in [NetAiOpsDbContext](../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L13-L300), and dashboard metrics are read from the database in [DashboardService](../src/Infrastructure/Persistence/DashboardService.cs#L8-L53).

The old database schema doc lists most operational tables as planned future work in [database-schema.md](../docs/database-schema.md#L20-L55). Those tables now exist as implementation-backed concepts: ticket operations are handled in [OperationsService](../src/Infrastructure/Operations/OperationsService.cs#L16-L191), incident and investigation behavior is implemented in [OperationsService](../src/Infrastructure/Operations/OperationsService.cs#L194-L360), and seeded scenarios populate users, incidents, tickets, logs, runbooks, agent runs, evals, approvals, and settings in [DatabaseSeeder](../src/Infrastructure/Persistence/DatabaseSeeder.cs#L21-L248).

The old roadmap marks Phase 2 through Phase 7 as planned in [implementation-roadmap.md](../docs/implementation-roadmap.md#L15-L56). The repo now contains a production-realism backbone across several of those areas: middleware and rate limiting in [Program](../src/Api/Program.cs#L19-L31), problem responses in [ExceptionHandlingMiddleware](../src/Api/Middleware/ExceptionHandlingMiddleware.cs#L6-L43), correlation and security headers in [CorrelationAndSecurityHeadersMiddleware](../src/Api/Middleware/CorrelationAndSecurityHeadersMiddleware.cs#L3-L23), an outbox worker in [OutboxWorker](../src/Infrastructure/Operations/OutboxWorker.cs#L9-L52), and CI in [ci.yml](../.github/workflows/ci.yml#L1-L30).

The first upskill README identified the repo as a uniform Phase 1 vertical slice in [docs/upskill/README.md](../docs/upskill/README.md#L5-L13). That remained useful as a provenance note, but the current training material should teach a broader internal platform rather than a single login-dashboard slice.

## What Remained Relevant

The old docs are still useful for understanding the project origin: layered backend, React frontend, JWT login, seeded users, protected routes, and the learning-focused tone. The dependency direction described in [architecture.md](../docs/architecture.md#L5-L15) still matches the code. The caution that juniors should start from workflows before data modeling in [database-schema.md](../docs/database-schema.md#L57-L59) is even more relevant now because the schema has grown.

The earlier tradeoffs also still matter. Startup migrations are convenient for a lab but risky in production because seeding calls `MigrateAsync` during app startup in [DatabaseSeeder](../src/Infrastructure/Persistence/DatabaseSeeder.cs#L16-L18). Frontend session storage remains a teaching tradeoff because protected calls rely on bearer tokens in [api.ts](../frontend/src/app/api.ts#L24-L41) and role-aware navigation in [AppLayout](../frontend/src/components/AppLayout.tsx#L5-L73).

## How Docs 2.0 Updates The Learning Path

Docs 2.0 treats the repo as a realistic mid-size-company codebase-in-miniature. It adds anchored flow maps for tickets, incidents, logs, agents, outbox, dashboard metrics, and role checks. It also calls out the remaining production gaps: integration tests, richer validation, background worker failure handling, database migration discipline, agent eval depth, and frontend mutation coverage.

Verified state during this pass: `dotnet test NetAiOps.sln` passed with 5 tests, `npm run build` passed, and `docker compose config` validated the default Postgres profile. Full command notes are in [verification log](08-reference/verification-log.md).
