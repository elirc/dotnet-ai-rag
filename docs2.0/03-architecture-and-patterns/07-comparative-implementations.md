# Comparative Implementations

## Direct EF Core Service vs Repository

Current choice: [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L16-L390) uses `DbContext` and LINQ directly. This is clear and efficient for a small team.

Alternative: repository/query objects. This can reduce service size and improve reuse, but often hides useful EF query details and adds abstraction too early.

Use repository/query objects when repeated query shapes or ownership boundaries emerge.

## Controller/Service vs Vertical Slice

Current choice: controllers delegate to a broad service, as in [TicketsController](../../src/Api/Controllers/TicketsController.cs#L13-L55). This is familiar to many .NET teams.

Alternative: vertical slice handlers per endpoint. This can make each workflow self-contained and easier to test, but it introduces more files and conventions.

Use vertical slices when endpoint behavior diverges heavily or when many teams own separate workflows.

## Outbox Worker vs Direct Side Effects

Current choice: service writes an outbox row through [AddOutboxMessage](../../src/Infrastructure/Operations/OperationsService.cs#L487-L493), and [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L9-L52) processes later. This protects against losing side effects when a request succeeds but an external call fails.

Alternative: send Slack/email/webhook inline. Easier initially, but less reliable and harder to retry.

## JWT Local Storage vs Secure Cookies

Current choice: frontend stores and sends bearer tokens through [api.ts](../../frontend/src/app/api.ts#L24-L41). This is simple for a lab.

Alternative: HTTP-only secure cookies plus CSRF protection. Better for many browser production apps, but requires more backend/frontend coordination.

## Startup Migrations vs Release Migrations

Current choice: [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L16-L18) runs migrations during startup. Good for local learning.

Alternative: migration job in CI/CD. Better for production because schema changes can be reviewed, ordered, monitored, and rolled back.
