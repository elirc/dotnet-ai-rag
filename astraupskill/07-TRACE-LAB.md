# Trace one incident into its saved records

Use the seeded billing incident in OperationsServiceTests as a controlled trace. Write down its identifier, primary service, detection time, log identifier, and knowledge-document identifier. Follow the command's role check before drawing any arrows to persistence. The request context is part of the input contract even though it is not a search term.

Mark the captured start time and calculate the lower log bound. The seeded log lies inside the interval and matches the exact service string. The service counts it and saves its identifier. Next construct the retrieval query from the incident fields and locate billing, capture, and timeout terms in the runbook. The precise score matters less than being able to account for every contribution using the title, tags, and content weights.

Now inspect the two tool records. SearchLogs should contain one sample identifier, while SearchKnowledge should contain the runbook identifier. Parse the parent input and output JSON and compare them with those children. A parent summary that names a document absent from the saved tool evidence would be a consistency defect.

Finish at the pending HumanApproval and incident candidate. Neither applies a mitigation. Record this distinction in your trace: retrieval completed, a recommendation exists, and execution has not been approved. Repeat the trace mentally with unrelated knowledge and then with a viewer caller. The first still produces a truthful no-match investigation; the second exits before creating the run. These three traces make the workflow's success, uncertainty, and rejection paths concrete.

## Source excerpt

From [tests/UnitTests/OperationsServiceTests.cs](../tests/UnitTests/OperationsServiceTests.cs).

```cs
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
        var incidentId = Guid.NewGuid();
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
