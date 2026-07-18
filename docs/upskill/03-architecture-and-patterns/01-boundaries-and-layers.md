# Boundaries And Layers

## Layer Ownership

| Layer | Owns | Must Not Own | Evidence |
| --- | --- | --- | --- |
| Domain | Core data shape | EF, HTTP, JWT, React | [AppUser](../../../src/Domain/Users/AppUser.cs#L3-L13) has no framework attributes. |
| Application | Contracts and DTOs | Database access, hashing, token signing | [DashboardSummaryDto](../../../src/Application/Dashboard/DashboardSummaryDto.cs#L3-L18) is pure data. |
| Infrastructure | EF, BCrypt, JWT implementation, seed data | Controller routing | [AddInfrastructure](../../../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L14-L29). |
| API | HTTP pipeline, auth policies, controllers | Business logic details | [AuthController](../../../src/Api/Controllers/AuthController.cs#L11-L17) delegates login. |
| Frontend | Browser UX and API calls | Server authorization | [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18). |

## Boundary Leaks To Watch

- [DashboardService](../../../src/Infrastructure/Persistence/DashboardService.cs#L5-L37) lives in Infrastructure but currently returns static data. That is fine for Phase 1, but when backed by tables, keep query logic testable.
- [Program seeding](../../../src/Api/Program.cs#L104-L109) performs database side effects at API startup. Convenient for training; risky for production multi-instance deployment.
- [authStore](../../../frontend/src/app/authStore.ts#L38-L52) stores the JWT in localStorage. Convenient for a lab; raises XSS blast-radius concerns.

## Drill

Pick a future ticket feature. Decide which layer owns: DTO, entity, EF mapping, authorization policy, controller route, React page, and test. Explain one thing each layer must not do.

