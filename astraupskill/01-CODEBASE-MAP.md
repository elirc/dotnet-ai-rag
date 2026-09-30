# Follow the investigation through the codebase

The entry into this change is OperationsService.RunIncidentInvestigationAsync. It accepts the authenticated RequestContext and incident identifier. Its first branch checks IsAdminOrEngineer, before looking up the incident or constructing an AgentRun. A permitted request for a missing incident returns null. These branches explain why the test fixture needs both an incident and a meaningful caller role.

The service queries Logs using the primary service name and a fixed time interval. It records the total match count separately from a bounded sample of at most twenty identifiers. That separation prevents a sample size from being presented as the total evidence population. Timestamp and identifier ordering make the selected sample stable when timestamps tie.

KnowledgeDocuments supply title, tags, and content to the application-layer LexicalRetriever. The retriever has no dependency on EF, HTTP, or a model provider. Its records make the handoff inspectable: RetrievalDocument describes an input document, while RetrievalMatch carries the identifier, title, heuristic score, and matched terms.

The service then builds an AgentRun with SearchLogs and SearchKnowledge children, a HumanApproval, an incident candidate summary, and an audit event. SaveChangesAsync persists those tracked changes. The unit test project contains direct ranking cases and a separate service fixture. The JSON evaluation file is copied to the test output and actually loaded by a test; it is not merely a decorative example. Follow these files in this order when diagnosing a changed recommendation.

## Source excerpt

From [src/Infrastructure/Operations/OperationsService.cs](../src/Infrastructure/Operations/OperationsService.cs).

```cs
    public async Task<AgentRunDto?> RunIncidentInvestigationAsync(RequestContext context, Guid id, CancellationToken ct)
    {
        if (!context.IsAdminOrEngineer)
        {
            throw new UnauthorizedAccessException("Only Admin and Engineer users can run investigation agents.");
        }

        var incident = await db.Incidents.SingleOrDefaultAsync(x => x.Id == id, ct);
        if (incident is null)
        {
            return null;
        }

        var startedAt = DateTimeOffset.UtcNow;
        var logWindowStart = incident.DetectedAt.AddHours(-2);
        var logWindowEnd = startedAt;
        var logQuery = db.Logs.AsNoTracking().Where(x =>
            x.ServiceName == incident.PrimaryService &&
            x.Timestamp >= logWindowStart &&
            x.Timestamp <= logWindowEnd);
        var evidenceCount = await logQuery.CountAsync(ct);
        var evidenceIds = await logQuery.OrderByDescending(x => x.Timestamp)
            .ThenBy(x => x.Id)
            .Select(x => x.Id)
            .Take(20)
            .ToListAsync(ct);
        var logCompletedAt = DateTimeOffset.UtcNow;

        var retrievalStartedAt = DateTimeOffset.UtcNow;
        var retrievalQuery = string.Join(' ', new[]
        {
            incident.PrimaryService,
            incident.Title,
            incident.Description,
            incident.AffectedServicesCsv ?? string.Empty
        });
        var documents = await db.KnowledgeDocuments.AsNoTracking()
            .Select(x => new RetrievalDocument(x.Id, x.Title, x.TagsCsv, x.Content))
            .ToListAsync(ct);
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
