# Mid-Level Feature Tickets

## Ticket 1: Ticket CRUD Vertical Slice

**Difficulty:** Hard **Time:** 2-4 days.

**Story:** As a support analyst, I want to create and view tickets so operations work can be tracked.

**Read first:** [App future routes](../../../frontend/src/app/App.tsx#L8-L19), [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L6-L32), [DashboardSummaryDto](../../../src/Application/Dashboard/DashboardSummaryDto.cs#L3-L18).

**Plan:** entity, migration, DTOs, service, controller, frontend list/detail/create, tests, dashboard count update.

**Risk:** status transitions become stringly typed; add enums and tests.

## Ticket 2: Replace Dashboard Placeholder Metrics

Read first: [DashboardService](../../../src/Infrastructure/Persistence/DashboardService.cs#L5-L37).

Acceptance: counts are database-backed, queries are bounded, tests cover empty DB and seeded DB.

## Ticket 3: Admin User List UI

Read first: [UsersController](../../../src/Api/Controllers/UsersController.cs#L7-L17) and [App route placeholder](../../../frontend/src/app/App.tsx#L17-L18).

Acceptance: Admin can list users; non-admin blocked by API; UI handles 403.

## Ticket 4: Auth Integration Test Harness

Read first: [Program](../../../src/Api/Program.cs#L13-L111).

Acceptance: test project can boot API with test configuration and fake/test DB.

