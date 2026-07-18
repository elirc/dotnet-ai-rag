# Systematic Debugging

Method: reproduce -> narrow -> hypothesize -> test cheaply -> fix root cause -> add regression coverage.

## Scenario: Login Returns 401

**Reproduction:** submit seeded credentials on [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L45-L59).

**First question:** client request problem or server credential problem?

**Narrowing path:**

1. Check network request body from [api.login](../../../frontend/src/app/api.ts#L29-L33).
2. Break/log in [AuthController.Login](../../../src/Api/Controllers/AuthController.cs#L13-L17).
3. Check normalized email lookup in [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L14-L17).
4. Confirm seed users exist from [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L14-L40).

**Regression test to add:** integration test for seeded admin login.

**Senior lesson:** avoid guessing at auth; split request, lookup, hash, token.

## Scenario: Dashboard Redirects To Login After Sign In

Narrow [authStore.setSession](../../../frontend/src/app/authStore.ts#L38-L47), [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18), and navigation in [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L29-L34).

## Scenario: Dashboard API Returns 401

Check bearer header in [request](../../../frontend/src/app/api.ts#L10-L18), JWT validation in [Program](../../../src/Api/Program.cs#L29-L44), and `[Authorize]` in [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17).

## Scenario: API Fails On Startup

Check Docker/Postgres availability. Startup seeding calls [DatabaseSeeder.SeedAsync](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L11) from [Program](../../../src/Api/Program.cs#L104-L109). Verified Docker failed in this environment.

## Scenario: Frontend Build Fails

Use [package scripts](../../../frontend/package.json#L6-L10). Classify the error as TypeScript, Vite, dependency, or environment.

