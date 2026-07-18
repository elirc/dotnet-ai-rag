# Side Effects Async And Reliability

The service writes audit events and outbox messages when workflows mutate state. Ticket creation, status updates, assignment, and comments call audit/outbox helpers in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L76-L191). Incident investigation creates timeline, agent, approval, audit, and outbox records in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L303-L360).

The outbox pattern keeps side effects durable. [AddOutboxMessage](../../src/Infrastructure/Operations/OperationsService.cs#L487-L493) writes a pending message in the same database transaction as the business change. [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L32-L52) later marks pending messages processed.

Current limitation: the worker simulates processing by marking rows as processed. A production worker would include retry counts, last error, backoff, dead-letter handling, metrics, and idempotency keys.
