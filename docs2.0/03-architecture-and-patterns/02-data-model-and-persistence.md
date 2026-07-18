# Data Model And Persistence

[NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L13-L300) is the authoritative persistence map. It includes users, tickets, comments, audit, incidents, timeline, logs, knowledge documents, agent runs, tool calls, eval runs, audit events, outbox messages, admin settings, and human approvals.

The model separates tickets from incidents. Tickets represent support work; incidents represent coordinated production response. That separation is reinforced by different service methods in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L16-L191) and [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L194-L360).

Current production gap: startup seeding calls `MigrateAsync` in [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L16-L18). That is convenient for a lab, but production systems usually run migrations as a release step with rollback planning.
