# Refactor And Design Katas

## Kata 1: Extract Ticket Policy

Current logic lives inside [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L517-L543). Extract it only if doing so improves testability without hiding domain meaning.

## Kata 2: Query Object For Logs

[OperationsService.SearchLogsAsync](../../src/Infrastructure/Operations/OperationsService.cs#L439-L466) is a compact query today. Design a `LogSearchQuery` object if filters grow to include environment, host, incident, trace, time range, and exception type.

## Kata 3: Dashboard Projection

[DashboardService.GetSummaryAsync](../../src/Infrastructure/Persistence/DashboardService.cs#L8-L53) computes metrics live. Design a cached projection approach and list what could become stale.
