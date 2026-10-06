# Pattern Catalog

Composition root: [Program](../../src/Api/Program.cs#L15-L137) centralizes service registration and middleware order.

Thin controller: [DashboardController](../../src/Api/Controllers/DashboardController.cs#L7-L17) contains almost no business logic.

Application contract: [OperationsDtos](../../src/Application/Operations/OperationsDtos.cs#L5-L115) defines public shapes independently from EF mapping details.

Service boundary: [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L14-L569) coordinates domain workflows, persistence, authorization checks, audit, and outbox writes.

Resource authorization: [CanReadTicket](../../src/Infrastructure/Operations/OperationsService.cs#L517-L528) allows broader access to Admin/Engineer and scoped access for SupportAnalyst.

Outbox: [AddOutbox](../../src/Infrastructure/Operations/OperationsService.cs#L559-L569) plus [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L9-L52).

Role-aware shell: [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73) filters UI affordances while backend policies remain authoritative.
