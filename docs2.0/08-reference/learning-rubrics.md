# Learning Rubrics

## Junior

Can trace a request from page to API to service to database. Example target: [TicketsPage](../../frontend/src/pages/TicketsPage.tsx#L6-L29) to [OperationsService.ListTicketsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L18-L60).

## Mid-Level

Can modify behavior with tests and explain tradeoffs. Example target: status transitions in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L122-L147) and capability tests in [RequestContextTests](../../tests/UnitTests/RequestContextTests.cs#L9-L29).

## Senior

Can identify ownership boundaries, failure modes, and migration paths. Example target: splitting [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L14-L569), hardening [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L9-L52), and replacing startup migrations in [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L16-L18).
