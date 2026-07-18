# System Map

## Project Graph

| Project/Folder | Owns | Evidence |
| --- | --- | --- |
| `src/Domain` | Core entities/enums without framework dependencies | [AppUser](../../../src/Domain/Users/AppUser.cs#L3-L13) is a plain class. |
| `src/Application` | DTOs and service contracts | [DashboardSummaryDto](../../../src/Application/Dashboard/DashboardSummaryDto.cs#L3-L18) is transport-oriented data. |
| `src/Infrastructure` | EF Core, auth implementation, seeding | [AddInfrastructure](../../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L14-L29) registers DbContext and services. |
| `src/Api` | HTTP pipeline and controllers | [Program](../../../src/Api/Program.cs#L13-L111) composes middleware and maps controllers. |
| `frontend` | Browser UI and API client | [main.tsx](../../../frontend/src/main.tsx#L8-L25) wires QueryClient, Router, and App. |
| `tests/UnitTests` | Current unit tests | [BCryptPasswordHasherTests](../../../tests/UnitTests/BCryptPasswordHasherTests.cs#L6-L18) verifies hashing behavior. |

## Public vs Private Surfaces

Public surfaces are controller routes, React routes, environment/config keys, and the migration schema. Private surfaces are service implementations and component internals.

| Surface | Public Contract | Anchor |
| --- | --- | --- |
| Login API | `POST /api/auth/login` | [AuthController.Login](../../../src/Api/Controllers/AuthController.cs#L11-L17) |
| Dashboard API | `GET /api/dashboard/summary` with JWT | [DashboardController.Summary](../../../src/Api/Controllers/DashboardController.cs#L7-L17) |
| Users API | `GET /api/users` for Admin policy | [UsersController.List](../../../src/Api/Controllers/UsersController.cs#L7-L17) |
| Frontend routes | `/login`, `/dashboard`, future placeholders | [App](../../../frontend/src/app/App.tsx#L21-L45) |
| Database table | `users` with unique email | [InitialIdentity.Up](../../../src/Infrastructure/Persistence/Migrations/20260614190221_InitialIdentity.cs#L14-L36) |

## Boundary Rule

Controllers should call application contracts, not EF directly. Infrastructure may know EF and BCrypt. Domain should not know HTTP, EF, React, or JWT.

