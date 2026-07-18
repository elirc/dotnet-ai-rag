# Side Effects, Async, And Reliability

## Current Async Boundaries

- [AuthController.Login](../../../src/Api/Controllers/AuthController.cs#L13-L17) is async and accepts `CancellationToken`.
- [AuthService.LoginAsync](../../../src/Infrastructure/Auth/AuthService.cs#L12-L24) passes the token to EF.
- [DatabaseSeeder.SeedAsync](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L40) performs migration and insert side effects.
- [DashboardService.GetSummaryAsync](../../../src/Infrastructure/Persistence/DashboardService.cs#L7-L35) returns `Task.FromResult` because data is static.

## Reliability Risks

| Risk | Evidence | Improvement |
| --- | --- | --- |
| Startup depends on DB availability | [Program seeding](../../../src/Api/Program.cs#L104-L109) | Separate migration job or explicit dev-only seeding. |
| No retry/backoff around DB startup | [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L11) | Add operational readiness guidance before production. |
| No request correlation middleware | [Program pipeline](../../../src/Api/Program.cs#L88-L102) | Add correlation ID before observability claims. |
| Placeholder async masks future latency | [DashboardService](../../../src/Infrastructure/Persistence/DashboardService.cs#L5-L37) | Add measured DB queries and tests when Phase 2 lands. |

## Drill

If login hangs under load, list three places to probe: browser network tab, ASP.NET logs around [AuthController.Login](../../../src/Api/Controllers/AuthController.cs#L13-L17), and EF query logging around [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L14-L15).

