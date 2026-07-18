# Domain Glossary

Ticket: a support work item. Ticket list, read, create, status, assignment, and comment behavior lives in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L16-L191).

Incident: a coordinated production response. Incident list, details, creation, timeline, and investigation behavior lives in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L194-L360).

Synthetic log: seeded operational telemetry that can be filtered by service, level, and query through [LogsController](../../src/Api/Controllers/LogsController.cs#L13-L17) and [OperationsService.SearchLogsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L363-L390).

Knowledge document: a runbook or process document. Seeded runbooks include billing, auth, incident response, and postmortem templates in [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L137-L144).

Agent run: a persisted execution of an AI-like workflow. The incident investigation flow creates an agent run and tool-call summary in [OperationsService.RunIncidentInvestigationAsync](../../src/Infrastructure/Operations/OperationsService.cs#L303-L360).

Human approval: a queue item requiring a person before risky action. The current investigation flow creates one in [OperationsService.RunIncidentInvestigationAsync](../../src/Infrastructure/Operations/OperationsService.cs#L341-L350).

Outbox message: a durable side-effect request. The service writes outbox rows through [AddOutboxMessage](../../src/Infrastructure/Operations/OperationsService.cs#L487-L493), and [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L32-L52) marks pending messages processed.

Eval run: a quality signal for retrieval or agent behavior. The frontend displays eval runs in [EvalsPage](../../frontend/src/pages/EvalsPage.tsx#L6-L29), and seed data creates two examples in [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L185-L214).
