# Use each test at its proper boundary

The ranking tests provide quick feedback without a database. They cover billing and authentication queries, top-k selection, ordering ties, unrelated words, and stop-word-only input. The evaluation test deserializes the checked-in JSON cases and applies their expected title or match count. If a case fails, inspect tokens and field membership before adjusting weights. A lower threshold can make a positive example pass while introducing unrelated matches elsewhere.

The service fixture seeds an incident, one matching log, and one relevant knowledge document in an isolated EF InMemory context. A deliberately awkward service name includes quotes and a backslash. The test parses the saved JSON, rather than matching an escaped string, and checks identifiers, counts, timestamps, model metadata, tool children, approval, and audit. A separate viewer test checks that authorization failure leaves no run or approval.

InMemory is useful for these service assertions but does not emulate relational transactions or every provider translation rule. A deployment confidence claim would need a separate PostgreSQL integration environment. These tests do not make that claim.

On this host, the initial test process could not find .NET 8 because it selected the global .NET 10 installation. The successful rerun uses the installed local .NET 8 toolchain and records the exact command. Treat a missing runtime as a runner failure, not a failing domain assertion. The verification chapter links accepted command evidence and leaves unsuccessful attempts in the campaign history.

## Source excerpt

From [tests/UnitTests/OperationsServiceTests.cs](../tests/UnitTests/OperationsServiceTests.cs).

```cs
        var service = new OperationsService(db);
        var result = await service.RunIncidentInvestigationAsync(
            new RequestContext(userId, "Engineer"), incidentId, CancellationToken.None);

        result.Should().NotBeNull();
        var run = await db.AgentRuns.Include(x => x.ToolCalls).SingleAsync();
        run.TotalToolCalls.Should().Be(2);
        run.TotalEstimatedTokens.Should().Be(0);
        run.EstimatedCost.Should().Be(0m);
        run.CompletedAt.Should().BeOnOrAfter(run.StartedAt);
        run.ToolCalls.Should().HaveCount(2);
        run.ToolCalls.Should().OnlyContain(call => call.CompletedAt >= call.StartedAt);

        using var input = JsonDocument.Parse(run.InputJson);
        input.RootElement.GetProperty("service").GetString().Should().Be(serviceName);
        var windowStart = input.RootElement.GetProperty("logWindowStart").GetDateTimeOffset();
        var windowEnd = input.RootElement.GetProperty("logWindowEnd").GetDateTimeOffset();
        windowStart.Should().Be(detectedAt.AddHours(-2));
        windowEnd.Should().BeOnOrAfter(detectedAt);
        windowEnd.Should().BeOnOrBefore(run.CompletedAt!.Value);

        using var output = JsonDocument.Parse(run.OutputJson);
        output.RootElement.GetProperty("modelUsed").GetBoolean().Should().BeFalse();
        output.RootElement.GetProperty("modelConfidence").ValueKind.Should().Be(JsonValueKind.Null);
        output.RootElement.GetProperty("evidenceCount").GetInt32().Should().Be(1);
        var match = output.RootElement.GetProperty("runbookMatches").EnumerateArray().Single();
        match.GetProperty("documentId").GetGuid().Should().Be(documentId);
        match.GetProperty("heuristicScore").GetInt32().Should().BeGreaterThan(0);

        var logCall = run.ToolCalls.Single(call => call.ToolName == "SearchLogs");
        using var logOutput = JsonDocument.Parse(logCall.OutputJson);
        logOutput.RootElement.GetProperty("sampleLogIds").EnumerateArray().Single().GetGuid().Should().Be(logId);
        var knowledgeCall = run.ToolCalls.Single(call => call.ToolName == "SearchKnowledge");
        using var knowledgeOutput = JsonDocument.Parse(knowledgeCall.OutputJson);
        knowledgeOutput.RootElement.GetProperty("matches").EnumerateArray().Single()
            .GetProperty("documentId").GetGuid().Should().Be(documentId);

        var approval = await db.HumanApprovals.SingleAsync();
        approval.Status.Should().Be("Pending");
        JsonDocument.Parse(approval.RecommendationJson).Dispose();
        (await db.AuditEvents.SingleAsync()).EventType.Should().Be("AgentRunCreated");
    }

    [Fact]
    public async Task Investigation_rejects_viewer_without_persisting_run_or_approval()
    {
        await using var db = CreateDatabase();
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
