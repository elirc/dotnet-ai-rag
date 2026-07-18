# Interview Prep From This Repo

Use this repo to practice production design answers:

1. Explain why tickets and incidents are separate. Anchor your answer in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L16-L360) and [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L53-L137).
2. Explain the outbox pattern using [AddOutboxMessage](../../src/Infrastructure/Operations/OperationsService.cs#L487-L493) and [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L9-L52).
3. Explain why frontend role checks are not security. Use [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73), [Program](../../src/Api/Program.cs#L69-L74), and [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L441-L467).
4. Explain what you would test next. Use [RequestContextTests](../../tests/UnitTests/RequestContextTests.cs#L7-L30) as the current baseline.

Strong interview answers include tradeoffs, failure modes, and one concrete improvement.
