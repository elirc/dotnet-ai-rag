# Testing Strategy

Current coverage is small: when this was written, `dotnet test NetAiOps.sln` passed with 5 tests. *(Update 2026-10-06: the suite is now 11 test cases: 7 `[Fact]` + one `[Theory]` × 4 rows, adding `LexicalRetrieverTests` and `OperationsServiceTests`. See [astraupskill/04](../../astraupskill/04-TESTING-AND-DEBUGGING.md).)* Existing examples include [BCryptPasswordHasherTests](../../tests/UnitTests/BCryptPasswordHasherTests.cs#L6-L17) and [RequestContextTests](../../tests/UnitTests/RequestContextTests.cs#L7-L30).

Recommended next layers:

1. Unit tests for status transitions in [OperationsService](../../src/Infrastructure/Operations/OperationsService.cs#L122-L147).
2. Unit or integration tests for resource authorization in [CanReadTicket](../../src/Infrastructure/Operations/OperationsService.cs#L517-L528).
3. API integration tests for policies configured in [Program](../../src/Api/Program.cs#L69-L74).
4. Database-backed tests for [DashboardService](../../src/Infrastructure/Persistence/DashboardService.cs#L8-L53).
5. Frontend component tests for role-based navigation in [AppLayout](../../frontend/src/components/AppLayout.tsx#L5-L73).

Testing principle: every production behavior that blocks or permits work deserves at least one negative test.
