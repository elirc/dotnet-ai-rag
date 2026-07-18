# ADR 0001: JWT Bearer Auth For Training Foundation

## Status

Accepted for local training. Revisit before production deployment.

## Context

The platform needs seeded users, RBAC, and protected API calls. JWT bearer auth is simple for a Vite SPA and ASP.NET Core API.

## Decision

Use JWT bearer tokens with role claims for Phase 1 and production-hardening exercises.

## Consequences

- Frontend can call APIs with `Authorization: Bearer`.
- The app must protect against token theft, stale roles, and weak signing keys.
- A future production deployment should compare this against httpOnly cookie auth.
