# Side Effects Async And Reliability

The service writes audit events and outbox messages when workflows mutate state. Ticket creation, status updates, assignment, and comments all write audit rows, but only ticket creation, assignment, and incident creation call `AddOutbox` (lines 116, 164, 268), in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L78-L193). Incident investigation creates agent-run, approval, and audit records (no timeline event, no outbox message) in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L305-L436).

The outbox pattern keeps side effects durable. [AddOutbox](../../src/Infrastructure/Operations/OperationsService.cs#L559-L569) writes a pending message in the same database transaction as the business change. [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L32-L52) later marks pending messages processed.

Current limitation: the worker simulates processing by marking rows as processed. A production worker would include retry counts, last error, backoff, dead-letter handling, metrics, and idempotency keys.
