# Code Review Mindset

Ask in order:

1. Does it work?
2. Is it correct under failure?
3. Will it stay correct as the app grows?
4. Does it fit the repo's boundaries?
5. Is it kind to future maintainers?

## Repo Checklist

- Controllers remain thin like [AuthController](../../../src/Api/Controllers/AuthController.cs#L11-L17).
- Server authorization exists like [UsersController](../../../src/Api/Controllers/UsersController.cs#L7-L17).
- EF constraints are explicit like [NetAiOpsDbContext](../../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L12-L30).
- Frontend has loading/error states like [DashboardPage](../../../frontend/src/pages/DashboardPage.tsx#L13-L19).
- Tests cover behavior like [BCryptPasswordHasherTests](../../../tests/UnitTests/BCryptPasswordHasherTests.cs#L6-L18).

## Good Comment Shape

> Important: this change adds a protected UI route but no server-side policy. Please add `[Authorize]` on the API endpoint and a 401/403 integration test, following the pattern in `UsersController`.

