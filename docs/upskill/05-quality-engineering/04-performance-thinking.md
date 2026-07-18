# Performance Thinking

Measure first. This repo is too small for real performance conclusions, but the hotspots are visible.

| Hotspot | Evidence | Why It Could Matter | Suggested Measurement |
| --- | --- | --- | --- |
| Login query | [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L14-L15) | Identity paths get high traffic | EF query logs and index usage. |
| BCrypt cost | [BCryptPasswordHasher](../../../src/Infrastructure/Auth/BCryptPasswordHasher.cs#L5-L10) | Password hashing is intentionally CPU-expensive | Login latency under load. |
| Startup migration | [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L11) | Startup latency and deployment coupling | Startup timing logs. |
| Dashboard query cache | [main QueryClient](../../../frontend/src/main.tsx#L8-L15) | Stale time affects freshness | Browser network panel. |
| Future list endpoints | inferred from placeholder routes [App](../../../frontend/src/app/App.tsx#L8-L19) | Tickets/logs can become unbounded | Pagination and indexes. |

## Drill

When Phase 2 adds logs, require pagination before rendering thousands of rows. Explain the database, API, and React consequences.

