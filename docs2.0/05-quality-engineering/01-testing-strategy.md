# Testing Strategy

Current verified coverage is small: `dotnet test NetAiOps.sln` passed with 5 tests. Existing examples include [BCryptPasswordHasherTests](../../tests/UnitTests/BCryptPasswordHasherTests.cs#L6-L17) and [RequestContextTests](../../tests/UnitTests/RequestContextTests.cs#L7-L30).

Recommended next layers:

1. Unit tests for status transitions in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L120-L145).
2. Unit or integration tests for resource authorization in [CanReadTicket](../../src/Infrastructure/Operations/OperationsService.cs#L441-L452).
3. API integration tests for policies configured in [Program](../../src/Api/Program.cs#L69-L74).
4. Database-backed tests for [DashboardService](../../src/Infrastructure/Persistence/DashboardService.cs#L8-L53).
5. Frontend component tests for role-based navigation in [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73).

Testing principle: every production behavior that blocks or permits work deserves at least one negative test.
