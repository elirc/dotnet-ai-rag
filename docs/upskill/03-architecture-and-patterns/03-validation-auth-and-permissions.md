# Validation, Auth, And Permissions

## Current Auth Boundary

JWT validation is configured in [Program](../../../src/Api/Program.cs#L29-L44). Role policies are defined in [Program](../../../src/Api/Program.cs#L46-L51). Controllers apply those policies with attributes: dashboard requires any authenticated user in [DashboardController](../../../src/Api/Controllers/DashboardController.cs#L7-L17), while users require Admin in [UsersController](../../../src/Api/Controllers/UsersController.cs#L7-L17).

## Validation Boundary

[LoginRequest](../../../src/Application/Auth/LoginRequest.cs#L5-L7) uses data annotations. [LoginPage](../../../frontend/src/pages/LoginPage.tsx#L9-L12) uses Zod. This is good duplication: client validation improves UX; server validation protects the system.

## Junior Misses vs Senior Checks

| Concern | Junior Miss | Senior Check |
| --- | --- | --- |
| Client route guards | Treats [ProtectedRoute](../../../frontend/src/components/ProtectedRoute.tsx#L9-L18) as security | Confirms `[Authorize]` exists server-side. |
| Roles | Assumes role name in JWT is enough forever | Plans resource-level checks before tickets/incidents are multi-user. |
| JWT storage | Ignores localStorage risk | Connects [authStore](../../../frontend/src/app/authStore.ts#L38-L52) to XSS blast radius. |
| Login | Checks happy path only | Adds invalid password, inactive user, malformed request, and rate-limit tests. |

## Drill

Write a review comment for a hypothetical PR that adds `/api/admin/settings` without `[Authorize(Policy = "AdminOnly")]`.

