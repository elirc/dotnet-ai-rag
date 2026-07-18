# Interview Prep From This Repo

Target: mid-level interviews, with senior signal in tradeoff answers.

## C# / .NET Runtime

| Question | Junior Answer | Mid-Level Adds | Senior Includes |
| --- | --- | --- | --- |
| How does async flow through login? | Controller calls service async. | `CancellationToken` reaches EF in [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L12-L15). | Cancellation saves work but does not replace timeouts/retries/observability. |
| Why use BCrypt? | To hash passwords. | [BCryptPasswordHasher](../../../src/Infrastructure/Auth/BCryptPasswordHasher.cs#L5-L10) verifies salted hashes. | Cost factor, rate limiting, and breach response matter. |
| What happens if config is wrong? | App may fail. | [Program](../../../src/Api/Program.cs#L25-L27) requires JWT config. | Add options validation and secret management. |

## ASP.NET / EF / React

| Question | Junior | Mid-Level | Senior |
| --- | --- | --- | --- |
| Where is authorization enforced? | `[Authorize]`. | [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17) and policies in [Program](../../../src/Api/Program.cs#L46-L51). | Resource authorization is needed when data ownership appears. |
| How does EF know user schema? | DbContext maps it. | [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L12-L30) plus migration [InitialIdentity](../../../src/Infrastructure/Persistence/Migrations/20260614190221_InitialIdentity.cs#L14-L36). | Migration review is a production safety practice. |
| Why React Query? | It fetches data. | [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L5-L18) models loading/error states. | Server-state cache discipline reduces duplicate logic and race bugs. |

## Design Tradeoff Questions From Comparison Cards

1. You chose controllers over minimal APIs here. Defend it.
   - Senior answer: controllers fit enterprise teaching and visible auth attributes now; switch only if ceremony dominates and route organization remains controlled.
2. You used direct DbContext instead of repositories. Defend it.
   - Senior answer: EF already provides unit-of-work/query features; add query objects/repositories when duplication or test pain appears.
3. You stored JWT in localStorage. Defend or reject it.
   - Senior answer: acceptable lab shortcut; production needs threat model comparing XSS token theft vs cookie CSRF complexity.
4. You used React Query instead of hand-rolled fetch state. Defend it.
   - Senior answer: upcoming server-state-heavy pages need caching/loading/error consistency; pay query-key discipline once.

## Debugging Questions

- Login succeeds in API but UI returns to `/login`: inspect [authStore](../../../frontend/src/app/authStore.ts#L38-L52) and [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18).
- API fails at startup: inspect [Program seeding](../../../src/Api/Program.cs#L104-L109), [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L11), and Docker status.

## Behavioral Prompts

- Tell me about a time you reduced risk before adding a feature. Use automatic startup migration as your example.
- Tell me how you would review a PR that adds an admin endpoint. Use [UsersController](../../../src/Api/Controllers/UsersController.cs#L7-L17) as the standard.
- Tell me how you would explain client vs server auth to a junior. Use [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18) and [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17).

