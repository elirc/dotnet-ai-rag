# Runtime And Tooling Map

## .NET

The API targets .NET 8 and enables nullable references and implicit usings in [NetAiOps.Api.csproj](../../../src/Api/NetAiOps.Api.csproj#L3-L7). ASP.NET Core composition happens in [Program](../../../src/Api/Program.cs#L13-L111). EF Core uses PostgreSQL through [AddInfrastructure](../../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L18-L19).

## React

The frontend build scripts and dependencies live in [package.json](../../../frontend/package.json#L6-L27). Runtime composition starts at [main.tsx](../../../frontend/src/main.tsx#L8-L25): QueryClient, BrowserRouter, then App.

## Environment And Secrets

The dev connection string and JWT signing key are currently in [appsettings.json](../../../src/Api/appsettings.json#L2-L16). That is acceptable for a training lab; in a deployed system, the signing key and password belong in secret storage or environment variables.

## Runtime Boundaries

| Boundary | Owner | Evidence |
| --- | --- | --- |
| Browser -> API | `fetch` wrapper adds JSON body and optional bearer token | [request](../../../frontend/src/app/api.ts#L10-L27) |
| API -> services | Controllers depend on application interfaces | [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L10-L16) |
| Services -> database | Infrastructure service uses DbContext | [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L7-L15) |
| Startup -> database | Seeder runs migration and inserts users | [Program seeding](../../../src/Api/Program.cs#L104-L109) |

