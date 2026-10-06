# Architecture Critique

## Strongest Choices

| Choice | Evidence | Why It Helps |
| --- | --- | --- |
| Layered backend | [Program](../../../src/Api/Program.cs#L13-L16) composes infrastructure rather than implementing it inline | Keeps HTTP, persistence, and domain concepts separable. |
| Explicit JWT policies | [Program](../../../src/Api/Program.cs#L46-L51) | Gives future modules named authorization decisions. |
| Explicit EF mapping | [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L12-L30) | Makes schema reviewable. |
| Frontend route shell | [App](../../../frontend/src/app/App.tsx#L21-L43) | Lets later phases land into an existing navigation model. |
| Query loading/error states | [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L13-L19) | Prevents blank screens during failure. |

## Risks And Tradeoffs

| Priority | Risk | Evidence | Confirmed vs Hypothesis | Improvement |
| --- | --- | --- | --- | --- |
| High | Automatic DB migration at API startup | [Program](../../../src/Api/Program.cs#L104-L109) | Confirmed | Keep for dev; move production migrations to a job/runbook. |
| High | JWT signing key in appsettings | [appsettings](../../../src/Api/appsettings.json#L5-L9) | Confirmed | Environment secret plus options validation. |
| Medium | Dashboard metrics are placeholders | [DashboardService](../../../src/Infrastructure/Persistence/DashboardService.cs#L25-L35) | Confirmed | Mark UI as seeded/mock until Phase 2 data exists. |
| Medium | No integration tests for auth endpoints | [existing test](../../../tests/UnitTests/BCryptPasswordHasherTests.cs#L6-L18) only covers hashing | Confirmed | Add WebApplicationFactory tests. |
| Medium | localStorage JWT exposure | [authStore](../../../frontend/src/app/authStore.ts#L38-L52) | Confirmed | Harden XSS posture or consider httpOnly cookies. |
| Low | Future route placeholders can imply implemented modules | [futureRoutes](../../../frontend/src/app/App.tsx#L8-L19) | Confirmed | Keep labels explicit until implemented. |

## If I Owned This For Three Months

1. Add API integration tests around login, invalid login, dashboard auth, and Admin-only users.
2. Add environment-specific seed/migration policy.
3. Introduce a real dashboard read model when tickets/incidents land.
4. Add error handling middleware and correlation IDs before agent/RAG work.
5. Add security tests around JWT validation, inactive users, and role restrictions.

## Migration And Test Strategy

For each recommendation: add a failing test first if behavior is observable, make the smallest change, verify with `dotnet test` and `npm run build`, then update this risk register.

