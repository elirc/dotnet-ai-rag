# Mid-Level Feature Tickets

## Feature 1: Ticket Create UI

Backend support exists in [TicketsController.Create](../../src/Api/Controllers/TicketsController.cs#L26-L31) and [OperationsService.CreateTicketAsync](../../src/Infrastructure/Operations/OperationsService.cs#L76-L118). Build a frontend form that calls [createTicket](../../frontend/src/app/api.ts#L57-L58).

Acceptance criteria: form validation, optimistic or post-success refresh, error state, accessible labels, and build pass.

## Feature 2: Outbox Retry Model

Current worker marks pending messages processed in [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L32-L52). Add retry count, last error, and dead-letter status.

Acceptance criteria: migration, worker logic, tests for retry behavior, dashboard or admin visibility for failed messages.

## Feature 3: Incident Timeline UI

Backend details include incident timeline events in [OperationsService.GetIncidentAsync](../../src/Infrastructure/Operations/OperationsService.cs#L222-L225) and timeline mutation in [OperationsService.AddTimelineAsync](../../src/Infrastructure/Operations/OperationsService.cs#L271-L300). Build a detail page that shows timeline and lets permitted users add events.
