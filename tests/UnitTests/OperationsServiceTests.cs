using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetAiOps.Application.Common;
using NetAiOps.Domain.Incidents;
using NetAiOps.Domain.Knowledge;
using NetAiOps.Domain.Logs;
using NetAiOps.Infrastructure.Operations;
using NetAiOps.Infrastructure.Persistence;

namespace NetAiOps.UnitTests;

public sealed class OperationsServiceTests
{
    [Fact]
    public async Task Investigation_persists_truthful_serialized_evidence_and_pending_approval()
    {
        await using var db = CreateDatabase();
        var userId = Guid.NewGuid();
        var incidentId = Guid.NewGuid();
        var logId = Guid.NewGuid();
        var documentId = Guid.NewGuid();
        var detectedAt = DateTimeOffset.UtcNow.AddMinutes(-10);
        const string serviceName = "billing-api \"blue\" \\ primary";

        db.Incidents.Add(new Incident
        {
            Id = incidentId,
            Title = "Payment capture failures",
            Description = "Billing requests time out during capture.",
            Severity = IncidentSeverity.SEV2,
            Status = IncidentStatus.Investigating,
            StartedAt = detectedAt.AddMinutes(-5),
            DetectedAt = detectedAt,
            PrimaryService = serviceName,
            CreatedByUserId = userId,
            AffectedServicesCsv = "payments,billing",
            CreatedAt = detectedAt,
            UpdatedAt = detectedAt
        });
        db.Logs.Add(new SyntheticLog
        {
            Id = logId,
            Timestamp = detectedAt.AddMinutes(1),
            ServiceName = serviceName,
            Environment = "test",
            Level = "Error",
            Message = "capture timeout",
            TraceId = "trace-1",
            CorrelationId = "correlation-1",
            Host = "test-host",
            LinkedIncidentId = incidentId
        });
        db.KnowledgeDocuments.Add(new KnowledgeDocument
        {
            Id = documentId,
            Title = "Billing capture timeout runbook",
            SourceType = "Runbook",
            Content = "Inspect payment capture latency and the billing dependency.",
            TagsCsv = "billing,payments,timeout",
            CreatedByUserId = userId,
            CreatedAt = detectedAt,
            UpdatedAt = detectedAt
        });
        await db.SaveChangesAsync();

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
        var incidentId = Guid.NewGuid();
        db.Incidents.Add(new Incident
        {
            Id = incidentId,
            Title = "Test incident",
            Description = "Test description",
            Severity = IncidentSeverity.SEV3,
            Status = IncidentStatus.Detected,
            StartedAt = DateTimeOffset.UtcNow.AddMinutes(-5),
            DetectedAt = DateTimeOffset.UtcNow,
            PrimaryService = "test-service",
            CreatedByUserId = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        });
        await db.SaveChangesAsync();

        var service = new OperationsService(db);
        var act = () => service.RunIncidentInvestigationAsync(
            new RequestContext(Guid.NewGuid(), "Viewer"), incidentId, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
        (await db.AgentRuns.CountAsync()).Should().Be(0);
        (await db.HumanApprovals.CountAsync()).Should().Be(0);
    }

    private static NetAiOpsDbContext CreateDatabase()
    {
        var options = new DbContextOptionsBuilder<NetAiOpsDbContext>()
            .UseInMemoryDatabase($"operations-service-{Guid.NewGuid()}")
            .Options;
        return new NetAiOpsDbContext(options);
    }
}
