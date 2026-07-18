# Trace Tables

## Trace: UI To API Login

| Step | File | Value Shape | Owner | Transformation | Risk |
| --- | --- | --- | --- | --- | --- |
| Form submit | [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L45-L59) | email/password | UI | form -> mutation | client validation bypassable |
| Fetch | [api.login](../../../frontend/src/app/api.ts#L29-L33) | JSON | client API | object -> HTTP body | wrong base URL |
| HTTP | [AuthController](../../../src/Api/Controllers/AuthController.cs#L11-L17) | LoginRequest | API | route -> service | no standard error envelope |
| DB check | [AuthService](../../../src/Infrastructure/Auth/AuthService.cs#L14-L17) | AppUser/null | infrastructure | query + hash verify | no rate limit |
| Token | [JwtTokenService](../../../src/Infrastructure/Auth/JwtTokenService.cs#L17-L35) | claims/JWT | infrastructure | user -> token | secret handling |

## Trace: Dashboard Data

| Step | File | Value Shape | Owner | Transformation | Risk |
| --- | --- | --- | --- | --- | --- |
| Route guard | [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18) | token/null | UI | token -> route decision | UX-only |
| Query | [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L5-L18) | DashboardSummary | UI | query -> states | stale token |
| Endpoint | [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17) | DTO | API | service -> HTTP | no role tailoring |
| Service | [DashboardService](../../../src/Infrastructure/Persistence/DashboardService.cs#L25-L35) | static values | infrastructure | constants -> DTO | fake metrics |

## Trace: User Seed

| Step | File | Value Shape | Owner | Transformation | Risk |
| --- | --- | --- | --- | --- | --- |
| Startup | [Program](../../../src/Api/Program.cs#L104-L109) | scope | API | service scope -> seeder | DB dependency |
| Migration | [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L9-L11) | schema | infrastructure | migration apply | multi-instance race |
| Insert | [DatabaseSeeder](../../../src/Infrastructure/Persistence/DatabaseSeeder.cs#L22-L40) | AppUser rows | infrastructure | seed list -> rows | default password |

