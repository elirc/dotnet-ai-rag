# Performance Thinking

Current strengths: list endpoints clamp `pageSize` through constants in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L12-L19), and ticket/log queries use pagination in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L16-L58) and [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L363-L390).

Database mappings include indexes for common filters, such as ticket status/priority/created time in [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L53-L81), incident severity/status in [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L110-L137), and log service/level/timestamp in [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L139-L173).

Current risks: dashboard metrics issue several count queries in [DashboardService](../../src/Infrastructure/Persistence/DashboardService.cs#L8-L53). That is fine for a lab, but a large system may need materialized summaries or caching.

Exercise: add a service filter and date range to log search, then decide which index would support it.
