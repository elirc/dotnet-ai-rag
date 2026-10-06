# Maintainer Communication

When you ask for review, name the risk:

- "This changes ticket status transitions in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L122-L147); please check whether escalation rules still match support expectations."
- "This changes role visibility in [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73); backend policy should be reviewed separately."
- "This changes outbox processing in [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L32-L52); please check retry and idempotency behavior."

When you are blocked, include what you tried, what command failed, and the smallest decision you need.
