# Framework Mental Models

## ASP.NET Core Pipeline

[Program](../../../src/Api/Program.cs#L88-L102) builds the request pipeline. Order matters: HTTPS redirection, CORS, authentication, authorization, controllers. If `[Authorize]` fails unexpectedly, inspect both token validation in [Program](../../../src/Api/Program.cs#L29-L44) and route attributes such as [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17).

## Dependency Injection

Services are registered in [AddInfrastructure](../../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L14-L29). `DbContext` and app services are scoped, which fits per-request database work. Captive dependency bug: if a singleton later depends on scoped `NetAiOpsDbContext`, it may hold a request-scoped service beyond its lifetime.

## EF Core

[NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L6-L32) models the database. EF queries are deferred until executed; [SingleOrDefaultAsync](../../../src/Infrastructure/Auth/AuthService.cs#L14-L15) is where the query runs.

## React Render Model

[main.tsx](../../../frontend/src/main.tsx#L17-L25) mounts the React tree. State changes in [authStore.setSession](../../../frontend/src/app/authStore.ts#L38-L47) cause subscribed components to re-render. [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L5-L18) renders loading, error, or data based on query state.

## Drill

Explain why putting `fetch` directly in the body of `DashboardPage` would be wrong. Mention render repeat, side effects, and React Query.

