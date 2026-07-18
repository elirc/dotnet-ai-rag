# File Reading Order

## Junior Path

| Order | File | Why | Look For |
| --- | --- | --- | --- |
| 1 | [AppUser](../../../src/Domain/Users/AppUser.cs#L3-L13) | Small domain object | Required properties, role, timestamps. |
| 2 | [LoginRequest](../../../src/Application/Auth/LoginRequest.cs#L5-L7) | Basic validation shape | Data annotations as API validation input. |
| 3 | [AuthController](../../../src/Api/Controllers/AuthController.cs#L7-L18) | Thin controller | Route, anonymous access, response shape. |
| 4 | [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L12-L24) | Real credential logic | Normalize email, active user, verify hash. |
| 5 | [JwtTokenService](../../../src/Infrastructure/Auth/JwtTokenService.cs#L14-L35) | Token construction | Claims, expiry, signing key. |
| 6 | [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L16-L69) | UI form flow | Schema, mutation, session write, navigation. |
| 7 | [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18) | Client-side gate | Redirect is UX, not security. |
| 8 | [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L5-L68) | Query/render cycle | Loading/error/data states. |

## Mid-Level Path

| Order | File | Why | Look For |
| --- | --- | --- | --- |
| 1 | [Program](../../../src/Api/Program.cs#L13-L111) | Runtime composition | Middleware order, policies, CORS, seeding. |
| 2 | [AddInfrastructure](../../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L14-L29) | Dependency boundaries | Scoped services and DbContext lifetime. |
| 3 | [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L6-L32) | Persistence contract | Table names, index, required columns. |
| 4 | [InitialIdentity](../../../src/Infrastructure/Persistence/Migrations/20260614190221_InitialIdentity.cs#L14-L44) | Schema artifact | What actually reaches PostgreSQL. |
| 5 | [api.request](../../../frontend/src/app/api.ts#L10-L27) | Client API boundary | Error handling, auth header, method inference. |
| 6 | [authStore](../../../frontend/src/app/authStore.ts#L14-L52) | Session state | Expiry cleanup and localStorage tradeoff. |

## Senior Path

Read [risk register](../08-reference/risk-register.md), then compare [Program](../../../src/Api/Program.cs#L104-L109), [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L40), [DashboardService](../../../src/Infrastructure/Persistence/DashboardService.cs#L5-L37), and [appsettings](../../../src/Api/appsettings.json#L2-L16). Ask what these choices commit production to and where a real system would need safer operational controls.

