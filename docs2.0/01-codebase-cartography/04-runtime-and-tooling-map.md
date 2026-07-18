# Runtime And Tooling Map

## Local Runtime

The intended local runtime is Postgres plus API plus frontend. The README documents Postgres and app commands in [README](../../README.md#L34-L58). Docker Compose defines Postgres, optional API, and optional frontend services in [docker-compose.yml](../../docker-compose.yml#L1-L47). During this pass, `docker compose config` validated the default Postgres profile.

## API Runtime

[Program](../../src/Api/Program.cs#L15-L137) configures controllers, Swagger, EF Core, infrastructure services, CORS, JWT bearer auth, authorization policies, rate limiting, middleware, health checks, and startup seeding. The API uses `MapControllers` and `MapHealthChecks` in [Program](../../src/Api/Program.cs#L119-L130).

## Frontend Runtime

The React app uses the routes in [App](../../frontend/src/app/App.tsx#L16-L43). HTTP calls are centralized in [api.ts](../../frontend/src/app/api.ts#L24-L91). Pages use TanStack Query, visible in [DashboardPage](../../frontend/src/pages/DashboardPage.tsx#L5-L18), [TicketsPage](../../frontend/src/pages/TicketsPage.tsx#L6-L13), and [IncidentsPage](../../frontend/src/pages/IncidentsPage.tsx#L6-L14).

## CI Runtime

The CI workflow restores and tests .NET, installs frontend dependencies, and builds the frontend in [ci.yml](../../.github/workflows/ci.yml#L21-L30). That makes backend tests and frontend type/build checks the minimum merge gate.
