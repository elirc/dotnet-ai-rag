# Phase 1 Journal: Foundation

## Planning

The purpose of Phase 1 is to prove the platform shape before building complex workflows. A junior engineer should notice that we did not start with agents. Agents need identity, authorization, persisted data, and observable APIs around them. Foundation first gives later phases a stable place to land.

The vertical slice is:

1. Login as a seeded user.
2. Receive a JWT containing role information.
3. Access a protected dashboard endpoint.
4. Render dashboard data in the React shell.

## Implementation

Backend work:

- Created the layered .NET solution.
- Added `AppUser` and `UserRole` in the domain.
- Added application DTOs and service contracts.
- Added EF Core PostgreSQL context and initial migration.
- Added BCrypt password hashing.
- Added JWT generation and role-based policies.
- Added `AuthController`, `DashboardController`, and `UsersController`.

Frontend work:

- Created a Vite React TypeScript app.
- Added routing, query client, form validation, and Zustand session state.
- Built a login page with seeded credentials.
- Built a protected dashboard layout with module navigation.
- Added placeholder pages for later phases.

## Retrospective

The end state is a working foundation with clean dependency direction. Dashboard numbers are intentionally placeholder values because tickets, incidents, logs, agents, and evals do not exist yet. The controller contract is real, so Phase 2 can replace placeholder metrics with database-backed metrics without changing the frontend API shape.

The most important choice was keeping business concepts out of controllers. Controllers should translate HTTP into application calls; they should not become the place where product logic quietly accumulates.
