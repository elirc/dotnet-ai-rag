# Follow the investigation through the codebase

This course follows one command, "run an investigation on an incident", from the HTTP
request to the rows it saves. Learn this one path well before you branch out. Every other
mutation in `OperationsService` uses the same shape: check the role, load, change, add an
audit row, save once.

## The projects, as they actually are

| Project | What is really in it | Notes |
| --- | --- | --- |
| `src/Domain` | Plain entity classes (`Incident`, `AgentRun`, `HumanApproval`, `KnowledgeDocument`, …) with public setters | no EF or HTTP references |
| `src/Application` | DTOs, service interfaces, `RequestContext`, and the one piece of real logic: [`LexicalRetriever`](../src/Application/Retrieval/LexicalRetriever.cs) | the retriever is pure: no EF, no I/O |
| `src/Infrastructure` | `NetAiOpsDbContext`, migrations, seeder, auth, **`OperationsService` (654 lines; almost all behavior)**, `OutboxWorker` | `OutboxWorker` is registered as a hosted service *inside the API process* ([`InfrastructureServiceCollectionExtensions.cs:30`](../src/Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs#L30)) |
| `src/Api` | controllers, two middlewares, auth policies, startup | startup migrates + seeds when `SeedDatabaseOnStartup` is true ([`Program.cs:132-137`](../src/Api/Program.cs#L132-L137), [`DatabaseSeeder.cs:18`](../src/Infrastructure/Persistence/DatabaseSeeder.cs#L18)) |
| `src/Workers`, `src/Shared` | **only a `.csproj` each**, no source files | placeholders; don't go looking for worker code there |
| `tests/UnitTests` | 4 test files: 7 `[Fact]` + 1 `[Theory]` with 4 `[InlineData]` rows = 11 test cases | the only test project; CI runs it via `dotnet test NetAiOps.sln` ([`ci.yml`](../.github/workflows/ci.yml)) |

## The request path, hop by hop

| # | Hop | Where | What to notice |
| --- | --- | --- | --- |
| 1 | Route + policy | [`IncidentsController.cs:42-48`](../src/Api/Controllers/IncidentsController.cs#L42-L48) | `CanInvestigate` = roles Admin or Engineer ([`Program.cs:72`](../src/Api/Program.cs#L72)); a Viewer is stopped here with 403 before any service code runs |
| 2 | Claims → context | [`RequestContext.FromClaims`](../src/Application/Common/RequestContext.cs#L7-L14) | a missing role claim becomes `"Viewer"`, so the default is the least privilege |
| 3 | Role check, again | [`OperationsService.cs:307-310`](../src/Infrastructure/Operations/OperationsService.cs#L307-L310) | defense in depth: the service refuses even if a future controller forgets the policy. The thrown `UnauthorizedAccessException` becomes 403 in [`ExceptionHandlingMiddleware.cs:18`](../src/Api/Middleware/ExceptionHandlingMiddleware.cs#L18) |
| 4 | Load incident | [line 312](../src/Infrastructure/Operations/OperationsService.cs#L312) | **tracked** (no `AsNoTracking`), because lines 428-432 modify it. Missing → `null` → controller returns 404 |
| 5 | Log evidence | [lines 318-331](../src/Infrastructure/Operations/OperationsService.cs#L318-L331) | one predicate, two queries: a full `CountAsync` and a 20-id sample |
| 6 | Knowledge evidence | [lines 333-345](../src/Infrastructure/Operations/OperationsService.cs#L333-L345) | **every** knowledge document is loaded, then ranked in memory |
| 7 | Describe + persist | [lines 347-434](../src/Infrastructure/Operations/OperationsService.cs#L347-L434) | `AgentRun` + 2 `AgentToolCall`s + `HumanApproval` + incident candidate + `AuditEvent`, all in **one** `SaveChangesAsync` (line 434) |

KnowledgeDocuments supply title, tags, and content to the application-layer
`LexicalRetriever`. Its records make the hand-off inspectable: `RetrievalDocument`
describes an input document, and `RetrievalMatch` carries the identifier, title, heuristic
score, and matched terms ([`LexicalRetriever.cs:5-6`](../src/Application/Retrieval/LexicalRetriever.cs#L5-L6)).

## What is *not* on this path (and why that matters)

- **No model call.** Nothing in the method contacts an LLM. The output says so explicitly
  (`modelUsed = false`, `modelConfidence = null`, lines 360-361).
- **No approval decision.** The `HumanApproval` row is created with `Status = "Pending"`,
  but the only approvals endpoint is a `GET` list
  ([`ApprovalsController.cs:13-17`](../src/Api/Controllers/ApprovalsController.cs#L13-L17)).
  Nothing in the API can approve or reject it. The row also has no foreign key to the run or
  the incident ([`HumanApproval.cs`](../src/Domain/Operations/HumanApproval.cs)); the only
  link is the copied `RecommendationJson`.
- **No mitigation executes.** "Pending" is the end of the line.

## Exercise: map it yourself

*Goal:* be able to answer "where would I look?" for any part of this flow without
searching.
*Hint:* keep this chapter closed and use only the file tree plus `grep`.
**Check:** without looking back, write the file and line range for (a) the role policy
definition, (b) the service-level role check, (c) the single save, and (d) the place a
thrown `UnauthorizedAccessException` turns into an HTTP status. Compare with the table
above: `Program.cs:72`, `OperationsService.cs:307-310`, `OperationsService.cs:434`,
`ExceptionHandlingMiddleware.cs:18`. Then answer: if you deleted the `[Authorize(Policy =
"CanInvestigate")]` on line 43, what status would a Viewer get now, and from which line?
(Still 403, now from the service check plus the middleware mapping. The
`Investigation_rejects_viewer_without_persisting_run_or_approval` test at
[`OperationsServiceTests.cs:110-138`](../tests/UnitTests/OperationsServiceTests.cs#L110-L138)
pins the service half.)

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
