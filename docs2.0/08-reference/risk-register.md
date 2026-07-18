# Risk Register

| Risk | Evidence | Mitigation |
| --- | --- | --- |
| Service grows too broad | [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L12-L493) spans many domains | Split by workflow ownership when adding more mutations |
| Startup migration risk | [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L16-L18) runs migrations on startup | Move migrations to release pipeline for production |
| Outbox has weak failure model | [OutboxWorker](../../src/Infrastructure/Operations/OutboxWorker.cs#L32-L52) marks pending rows processed | Add retry count, last error, dead-letter status, metrics |
| Authorization gaps | Policies exist in [Program](../../src/Api/Program.cs#L69-L74), but few tests cover API paths | Add integration tests for each role and mutation |
| Frontend runtime validation gap | [api.ts](../../frontend/src/app/api.ts#L24-L91) trusts response shapes | Add Zod schemas or generated typed client |
| Dashboard query scale | [DashboardService](../../src/Infrastructure/Persistence/DashboardService.cs#L8-L53) performs live counts | Cache or project metrics if data grows |
