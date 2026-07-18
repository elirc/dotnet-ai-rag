# Pattern Catalog

## Pattern: Thin Controller

**Problem:** HTTP endpoints should not accumulate business logic.

**General shape:** controller validates routing/auth attributes, delegates to a service, returns HTTP response.

**Real example:** [AuthController.Login](../../../src/Api/Controllers/AuthController.cs#L11-L17)

**Second:** [DashboardController.Summary](../../../src/Api/Controllers/DashboardController.cs#L12-L16)

**Why it works:** The controller remains easy to read and test through integration tests.

**Failure modes:** Services become too large; errors are not standardized; controller skips authorization.

**Use when:** endpoint logic is mostly orchestration.

**Avoid when:** the endpoint is so tiny that a minimal API would be clearer for this codebase.

**Drill:** Add a hypothetical `HealthController` both as a controller and as a minimal API sketch. Compare readability.

## Pattern: Application Contract Interface

**Problem:** API needs to depend on a stable contract, not infrastructure details.

**General shape:** application interface + infrastructure implementation + DI registration.

**Real example:** [IDashboardService usage](../../../src/Api/Controllers/DashboardController.cs#L10-L16)

**Second:** [AddInfrastructure registrations](../../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L21-L26)

**Why it works:** Later tests can substitute fakes, and controllers do not know EF.

**Failure modes:** Too many one-method interfaces can add ceremony without test value.

**Use when:** implementation has side effects, persistence, or external dependencies.

**Avoid when:** code is simple pure calculation with no boundary pressure.

**Drill:** Decide whether `JwtTokenService` should stay behind [IJwtTokenService](../../../src/Application/Auth/IJwtTokenService.cs) as the auth module grows.

## Pattern: Options Binding

**Problem:** services need configuration without reading raw configuration everywhere.

**General shape:** bind config section once, inject typed options.

**Real example:** [Configure JwtOptions](../../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L14-L17)

**Second:** [JwtTokenService options](../../../src/Infrastructure/Auth/JwtTokenService.cs#L10-L16)

**Why it works:** Keeps token construction decoupled from configuration source.

**Failure modes:** Missing validation means bad config fails late or insecurely.

**Use when:** settings have a coherent shape and multiple fields.

**Avoid when:** a setting is a one-off constant used once.

**Drill:** Add an options validation rule for signing key length on paper.

## Pattern: EF Fluent Mapping

**Problem:** C# property names and database schema need explicit control.

**General shape:** map table, key, indexes, columns, lengths, conversions.

**Real example:** [NetAiOpsDbContext.OnModelCreating](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L10-L31)

**Second:** [InitialIdentity migration](../../../src/Infrastructure/Persistence/Migrations/20260614190221_InitialIdentity.cs#L14-L36)

**Why it works:** The schema is explicit and reviewable.

**Failure modes:** C# mapping and migration drift if migrations are not reviewed.

**Use when:** database naming and constraints matter.

**Avoid when:** not applicable; even small EF apps benefit from explicit constraints.

**Drill:** Explain why the unique email index belongs in the model, not just in service code.

## Pattern: Client Query State

**Problem:** UI data fetching needs loading, error, retry, and caching behavior.

**General shape:** QueryClient provider at app root, `useQuery` at page boundary.

**Real example:** [main QueryClient](../../../frontend/src/main.tsx#L8-L15)

**Second:** [DashboardPage useQuery](../../../frontend/src/pages/DashboardPage.tsx#L5-L18)

**Why it works:** The page states are explicit and reusable.

**Failure modes:** stale query keys, hidden retries, missing auth invalidation.

**Use when:** server state should be cached or refreshed.

**Avoid when:** state is purely local form state.

**Drill:** What should happen to cached dashboard data after sign-out? Use [clearSession](../../../frontend/src/app/authStore.ts#L48-L51) as the starting point.

## Pattern: Session Store

**Problem:** multiple components need user/token state.

**General shape:** Zustand store loads, writes, and clears local session.

**Real example:** [useAuthStore](../../../frontend/src/app/authStore.ts#L14-L52)

**Second:** [AppLayout consumes user](../../../frontend/src/components/AppLayout.tsx#L17-L25)

**Why it works:** Keeps auth state out of prop chains.

**Failure modes:** localStorage token exposure, stale role after server change, no refresh path.

**Use when:** lightweight global client state is enough.

**Avoid when:** state must be server-authoritative on every interaction.

**Drill:** Sketch how you would clear React Query cache on logout.

