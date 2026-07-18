# Fast Track

Goal: get one weekend win without pretending you understand the whole platform.

## Verified Commands

| Command | Result | Notes |
| --- | --- | --- |
| `& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln` | Verified pass | 1 xUnit test passed. See [verification log](08-reference/verification-log.md). |
| `cd frontend; npm run build` | Verified pass | TypeScript and Vite build passed. |
| `docker compose up -d` | Verified fail | Docker daemon unavailable on this machine. Do not claim local DB startup until Docker is running. |

## First 10 Files

1. [Program](../../src/Api/Program.cs#L13-L111) - API composition, middleware, auth, seeding.
2. [InfrastructureServiceCollectionExtensions](../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L14-L29) - DI and DbContext registration.
3. [AppUser](../../src/Domain/Users/AppUser.cs#L3-L13) - identity domain shape.
4. [NetAiOpsDbContext](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L6-L32) - EF mapping and index.
5. [DatabaseSeeder](../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L7-L44) - seed accounts and startup migration.
6. [AuthController](../../src/Api/Controllers/AuthController.cs#L7-L18) - login HTTP contract.
7. [AuthService](../../src/Infrastructure/Auth/AuthService.cs#L7-L25) - credential verification.
8. [JwtTokenService](../../src/Infrastructure/Auth/JwtTokenService.cs#L10-L37) - token claims and signing.
9. [LoginPage](../../frontend/src/pages/LoginPage.tsx#L16-L69) - React login flow.
10. [DashboardPage](../../frontend/src/pages/DashboardPage.tsx#L5-L68) - protected data fetch and render.

## Trace These Flows

- Login: [LoginPage](../../frontend/src/pages/LoginPage.tsx#L29-L34) -> [api.login](../../frontend/src/app/api.ts#L29-L33) -> [AuthController.Login](../../src/Api/Controllers/AuthController.cs#L11-L17) -> [AuthService.LoginAsync](../../src/Infrastructure/Auth/AuthService.cs#L12-L24).
- Dashboard: [ProtectedRoute](../../frontend/src/components/ProtectedRoute.tsx#L9-L18) -> [DashboardPage](../../frontend/src/pages/DashboardPage.tsx#L5-L18) -> [DashboardController.Summary](../../src/Api/Controllers/DashboardController.cs#L7-L17) -> [DashboardService.GetSummaryAsync](../../src/Infrastructure/Persistence/DashboardService.cs#L5-L37).

## One Small Safe Change

Change the text of one placeholder route description in [App](../../frontend/src/app/App.tsx#L8-L19), run `npm run build`, and explain why this does not change backend contracts.

## Teach-Back

In five minutes, explain: what is authenticated on the client, what is authorized on the server, and why both matter. Use [ProtectedRoute](../../frontend/src/components/ProtectedRoute.tsx#L9-L18) and [DashboardController](../../src/Api/Controllers/DashboardController.cs#L7-L17) as the contrast.

## What This Skips

It skips real database startup because Docker is unavailable, all future ticket/incident/RAG/agent modules because they are placeholders, and integration/API tests because no WebApplicationFactory test project exists yet.

