# 9. Read what the outbox worker actually processes

[OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs) periodically creates a service scope, loads up to ten Pending messages ordered by creation time, logs their identifiers and types, marks them Processed and saves changes. The inspected worker does not call an external delivery adapter. “Processed” therefore describes this current local bookkeeping path, not confirmed notification or mitigation delivery.

## Separate durable intent from effects

Ticket and incident creation add outbox records before SaveChanges. That can preserve an intent alongside the domain change under the configured persistence behavior. The worker later observes those records. To claim durable delivery, however, an implementation would need an actual destination, payload contract, acknowledgment and recovery policy. A status name alone does not supply those pieces.

Write a trace from CreateIncidentAsync through AddOutbox to the worker. List which data is stored and which operations occur when a message is processed. A good answer says that the current worker logs and updates status. It should not describe emails, webhooks or operational actions that are not present in the inspected method.

## Multiple workers

Two worker instances can query the same Pending messages before either saves Processed. The inspected selection does not include a claim transition, lease or explicit row-lock protocol. Because the current effect is logging and local status marking, the consequences differ from a future external delivery worker. The race still matters when designing that extension.

Create a proposed claim operation that conditionally changes Pending to Processing with owner and lease expiry. Explain how a crashed worker's lease is recovered and how a stale worker is prevented from completing after a newer owner takes over. A fencing token can distinguish owners if every relevant write or destination honors it; merely naming a field fence does not enforce anything by itself.

## Batch failures

The worker updates selected messages in memory and calls SaveChanges after the loop. A failure before that save can leave database statuses Pending. If external effects were added inside the loop, some could already have occurred before the batch save failed. Retrying the batch could repeat them. This is a classic boundary to address before extending the worker.

Compare per-message completion with batch completion. Per-message saves can reduce the amount of uncertain local progress but do not eliminate the gap after remote success and before local acknowledgment. A destination-enforced idempotency key or a reconciliation contract may be necessary. Define the key scope and changed-payload behavior rather than assuming every destination supports the same mechanism.

## Cancellation and loop lifetime

The worker checks the stopping token, passes it through processing and uses a cancellation-aware delay. OperationCanceledException during processing is handled specially when shutdown is requested. The delay is outside that inner catch structure, so inspect the BackgroundService lifecycle when designing exact shutdown tests. Do not treat cancellation as proof that every prior message effect was rolled back.

Use a fake clock or delay abstraction in a proposed isolated worker test to avoid waiting fifteen seconds repeatedly. Test shutdown before a batch, during a database query and after local status changes but before save. Record which operations completed. A test that only cancels an idle delay does not establish in-flight recovery behavior.

## Deliverable

Submit a current-behavior trace, a two-worker schedule and a proposed delivery protocol. Mark external delivery as future work. Include one crash after remote acceptance and explain how the design avoids blindly repeating an irreversible action. The learning objective is to understand what the outbox currently guarantees and what a real dispatcher would need to add.

[Index](README.md) · [Review notes](answers/05-approval-outbox-and-json.md)
