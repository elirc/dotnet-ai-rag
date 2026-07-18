# Good First Tickets

## Ticket 1: Add Options Validation For JWT Settings

**Difficulty:** Easy **Time:** 1-2 hours **Skills:** options, validation, startup failure.

**Story:** As an operator, I want invalid JWT config to fail at startup so insecure deployments do not run.

**Why it's good:** Small blast radius around [AddInfrastructure](../../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L14-L17) and [JwtOptions](../../../src/Infrastructure/Auth/JwtOptions.cs#L3-L11).

**Acceptance criteria:**

- [ ] Signing key length is validated.
- [ ] Issuer/audience required.
- [ ] `dotnet test` passes.

**Read first:** [Program JWT read](../../../src/Api/Program.cs#L25-L27).

**Files likely touched:** `src/Infrastructure/Auth/JwtOptions.cs`, `src/Infrastructure/DependencyInjection/...`.

**Review questions:** Does this fail before accepting requests?

## Ticket 2: Add AuthService Unit Tests

**Difficulty:** Easy **Time:** 2-3 hours **Skills:** service tests, fake dependencies.

**Story:** As a maintainer, I want login behavior covered beyond hashing.

**Read first:** [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L12-L24).

**Acceptance criteria:** valid user returns token; wrong password returns null; inactive user returns null.

## Ticket 3: Improve Dashboard Mock Labeling

**Difficulty:** Easy **Time:** 1 hour **Skills:** React copy, user trust.

**Story:** As a learner, I want placeholder metrics marked as mock data.

**Read first:** [DashboardService](../../../src/Infrastructure/Persistence/DashboardService.cs#L25-L35) and [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L32-L67).

**Acceptance criteria:** UI labels mock/seeded status without implying production data.

## Ticket 4: Clear Query Cache On Logout

**Difficulty:** Medium **Time:** 2-4 hours **Skills:** React Query, auth state.

**Read first:** [AppLayout.logout](../../../frontend/src/components/AppLayout.tsx#L22-L25) and [main QueryClient](../../../frontend/src/main.tsx#L8-L15).

**Acceptance criteria:** signing out clears session and server-state cache.

## Ticket 5: Add Dashboard Unauthorized Integration Test

**Difficulty:** Medium **Time:** 3-5 hours **Skills:** WebApplicationFactory, auth.

**Read first:** [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17).

**Acceptance criteria:** no token returns 401; valid token returns 200.

## Ticket 6: Document Docker Startup Failure Mode

**Difficulty:** Easy **Time:** 1 hour **Skills:** local dev docs.

**Read first:** [docker-compose](../../../docker-compose.yml#L1-L20) and [verification log](../08-reference/verification-log.md).

**Acceptance criteria:** local dev docs explain Docker daemon prerequisite and migration startup dependency.


