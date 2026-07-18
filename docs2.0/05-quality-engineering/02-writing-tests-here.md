# Writing Tests Here

Start with small pure behaviors. [RequestContextTests](../../tests/UnitTests/RequestContextTests.cs#L9-L29) is a good model because it creates claims, calls one method, and asserts capability flags.

Good next test:

```csharp
// Illustrative: not from this repo.
[Theory]
[InlineData("New", "Closed", false)]
[InlineData("Triaged", "Escalated", true)]
public void StatusTransitionRules_AreEnforced(...)
```

The production code to target is [CanTransition](../../src/Infrastructure/Operations/OperationsService.cs#L454-L467). Because it is currently private, you can test through `UpdateTicketStatusAsync` or decide whether extracting a small policy object is worth it.

Avoid over-mocking EF for query-heavy methods. For [DashboardService](../../src/Infrastructure/Persistence/DashboardService.cs#L8-L53), an integration test with a test database or SQLite-compatible shape will teach more than a mock.
