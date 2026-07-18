# Senior Build Projects

## Project 1: Production-Ready Auth Hardening

**Value:** reduce security risk before agents can perform risky actions.

**Files/modules:** [Program](../../../src/Api/Program.cs#L29-L51), [JwtTokenService](../../../src/Infrastructure/Auth/JwtTokenService.cs#L14-L35), [authStore](../../../frontend/src/app/authStore.ts#L38-L52), [UsersController](../../../src/Api/Controllers/UsersController.cs#L7-L17).

**Checklist:** options validation, secret loading, auth integration tests, rate limiting, audit events, token storage decision, role/resource authorization plan.

## Project 2: Operational Migration Strategy

**Value:** remove production risk from startup migrations.

**Files/modules:** [Program seeding](../../../src/Api/Program.cs#L104-L109), [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L40), [docker-compose](../../../docker-compose.yml#L1-L20).

**Checklist:** dev seeding remains easy; production migration job documented; rollback story; readiness probes.

## Project 3: Observability Baseline

**Value:** make future RAG/agent failures debuggable.

**Files/modules:** [Program pipeline](../../../src/Api/Program.cs#L88-L102), [AuthController](../../../src/Api/Controllers/AuthController.cs#L11-L17), [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L12-L16).

**Checklist:** correlation IDs, exception middleware, structured logs, health checks, auth failure metrics.

