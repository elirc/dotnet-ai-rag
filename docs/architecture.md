# Architecture

> **Status (verified 2026-10-06):** this page was written during Phase 1. Tickets, incidents, logs, knowledge documents, agent runs, approvals, eval runs, the outbox, and their EF migrations now exist, and the dashboard computes real counts from them (`src/Infrastructure/Persistence/DashboardService.cs`). The "Phase Boundaries" section below is historical. `src/Workers` and `src/Shared` are still empty placeholder projects; the `OutboxWorker` runs inside the API process as a hosted service.

NetAiOps is organized as a layered ASP.NET Core backend plus a React frontend.

## Backend Shape

- `src/Domain`: core entities and enums. This layer has no EF, HTTP, or infrastructure dependencies.
- `src/Application`: DTOs and service contracts. Controllers talk in terms of these contracts.
- `src/Infrastructure`: EF Core, PostgreSQL, password hashing, JWT token creation, seed data, and future external integrations.
- `src/Api`: HTTP composition layer with controllers, auth policies, Swagger, CORS, and startup seeding.
- `src/Workers`: placeholder project for later background queues and workers.
- `src/Shared`: placeholder for cross-cutting primitives that are not domain behavior.

The dependency direction is inward: API depends on Application and Infrastructure; Infrastructure depends on Application and Domain; Domain depends on nothing.

## Frontend Shape

The frontend lives in `frontend/` and uses React, TypeScript, Vite, React Router, TanStack Query, React Hook Form, Zod, and Zustand.

Phase 1 includes:

- Login page with Zod validation
- Session store backed by `localStorage`
- Protected route wrapper
- Enterprise dashboard layout
- Navigation placeholders for future modules
- TanStack Query API integration

## Runtime Flow

1. User signs in through `POST /api/auth/login`.
2. API validates the BCrypt password against the seeded user row.
3. API returns a JWT containing user id, email, display name, and role.
4. Frontend stores the session and sends `Authorization: Bearer ...` on protected API calls.
5. Dashboard page calls `GET /api/dashboard/summary`.

## Phase Boundaries

Phase 1 deliberately avoids implementing ticket and incident persistence. The dashboard currently returns realistic placeholder metrics through the same service contract that will later query Phase 2 tables. This lets the first vertical slice prove auth, routing, API composition, and frontend data fetching before the domain grows.
