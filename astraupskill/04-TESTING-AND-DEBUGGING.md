# Use each test at its proper boundary

## The whole suite, counted

One project, [`tests/UnitTests`](../tests/UnitTests/NetAiOps.UnitTests.csproj): 7 `[Fact]` +
1 `[Theory]` × 4 `[InlineData]` rows = **11 test cases** in 8 methods. That matches the 11
recorded in [VERIFICATION](VERIFICATION.md).

| Test | Lines | Boundary | What it actually pins |
| --- | --- | --- | --- |
| `Verify_ReturnsTrue_ForMatchingPassword` | [`BCryptPasswordHasherTests.cs:8-17`](../tests/UnitTests/BCryptPasswordHasherTests.cs#L8-L17) | pure | hash round-trips; a wrong password fails |
| `Billing_and_auth_queries_rank_the_matching_runbook_first` | [`LexicalRetrieverTests.cs:21-28`](../tests/UnitTests/LexicalRetrieverTests.cs#L21-L28) | pure | top-1 for two queries, both input orders |
| `Ranking_is_stable_bounded_and_uses_title_then_id_for_ties` | [30-42](../tests/UnitTests/LexicalRetrieverTests.cs#L30-L42) | pure | `topK: 1`, title tie-break, id tie-break |
| `Unrelated_or_stop_word_only_queries_return_no_match` | [44-49](../tests/UnitTests/LexicalRetrieverTests.cs#L44-L49) | pure | zero-overlap and stop-word-only queries |
| `Checked_in_evaluation_cases_are_loaded_and_pass` | [51-75](../tests/UnitTests/LexicalRetrieverTests.cs#L51-L75) | pure + file | the 3 cases in `sample-eval-cases/retrieval.json`, copied to the output dir by [`NetAiOps.UnitTests.csproj:32`](../tests/UnitTests/NetAiOps.UnitTests.csproj#L32) |
| `Investigation_persists_truthful_serialized_evidence_and_pending_approval` | [`OperationsServiceTests.cs:15-108`](../tests/UnitTests/OperationsServiceTests.cs#L15-L108) | EF InMemory | saved JSON parses; counts, ids, model metadata, 2 tool calls, pending approval, 1 audit row |
| `Investigation_rejects_viewer_without_persisting_run_or_approval` | [110-138](../tests/UnitTests/OperationsServiceTests.cs#L110-L138) | EF InMemory | Viewer → exception and zero rows |
| `FromClaims_MapsRoleCapabilities` (×4) | [`RequestContextTests.cs:9-29`](../tests/UnitTests/RequestContextTests.cs#L9-L29) | pure | the role → capability table for all four roles |

CI runs all of it with `dotnet test NetAiOps.sln` on `windows-latest`
([`ci.yml`](../.github/workflows/ci.yml)).

## What the suite does *not* pin

This is the part a mid-level engineer adds to a review. Reason it out against the code; you
don't need to run anything:

- **The weights and the threshold are unpinned.** Change the title weight from 4 to 2
  ([`LexicalRetriever.cs:44`](../src/Application/Retrieval/LexicalRetriever.cs#L44)) and
  every one of the 11 cases still passes (exercise 1 below proves it). Lower the default
  `minimumScore` from 3 to 1 and they *still* all pass. No test contains a content-only
  match that the threshold is supposed to drop.
- **The log window's edges are unpinned.** The service test has one log, inside the window.
  Nothing checks the lower bound (`>=`), the upper bound (`<=`), or the 20-id cap.
- **No HTTP test exists.** Policies (`CanInvestigate`), the exception → status mapping, and
  `RequestContext.FromClaims` wiring inside controllers are untested end to end.
- **Ticket visibility is untested.** `CanReadTicket`
  ([`OperationsService.cs:517-520`](../src/Infrastructure/Operations/OperationsService.cs#L517-L520))
  and the transition rules (lines 522-535) have no tests at all.

## What EF InMemory can and cannot tell you

The service fixture ([`CreateDatabase`, lines 140-146](../tests/UnitTests/OperationsServiceTests.cs#L140-L146))
uses a fresh, uniquely named InMemory database per test, so tests can't see each other's
rows. That is good for asserting *which objects the service saved and how they're linked
in memory*. It is not a relational database: there is no SQL, no transactions, and no
foreign-key enforcement. So these tests cannot show that the single `SaveChangesAsync` is
atomic in PostgreSQL, that the queries translate, or how they perform. A deployment
confidence claim would need a PostgreSQL integration test (extended lesson 24 designs one).
These tests don't make that claim, and you shouldn't either.

## Debugging playbook for this command

| Symptom | First probe | Usual cause |
| --- | --- | --- |
| `evidenceCount` is 0 but logs exist | compare `incident.PrimaryService` with `Logs.ServiceName` byte for byte | the filter is exact equality (line 322). `BillingService` ≠ `billing-service` |
| A runbook you expected is missing | tokenize the query and the document by hand (chapter 2) | split words (`time out` vs `timeout`), plural vs singular, or score below 3 |
| Wrong runbook ranked first | write each term's per-field points in a table | a word in the *title* (4 points) beats several in content (1 each) |
| 403 for an Engineer | decode the JWT and check the `role` claim | missing role claim → `FromClaims` defaults to `Viewer` ([`RequestContext.cs:11`](../src/Application/Common/RequestContext.cs#L11)) |
| 409 from run-investigation | was the incident's timeline edited at the same moment? | `AddIncidentTimelineAsync` advances `Version` (line 299); the investigation's save then fails its concurrency check → `DbUpdateConcurrencyException` → 409 ([`ExceptionHandlingMiddleware.cs:20`](../src/Api/Middleware/ExceptionHandlingMiddleware.cs#L20)) |

**Runner trouble is not a test failure.** All projects target `net8.0` and the repo has no
`global.json`. On a machine with only a newer SDK, the build can succeed while `dotnet test`
fails to *start* the test host because the .NET 8 runtime is missing. Install the .NET 8
runtime (or SDK) and rerun before reading anything into the result. A test that never ran
has neither passed nor failed.

## Exercises

**1 (easy) — prove the weights are unpinned.** *Goal:* see what the suite really protects.
Set the title weight to 2 in your head and recompute the two ranking queries and the
service fixture. *Check:* `BillingService timeout checkout` → Billing = 2 + (2+3) + 1 = 8,
still first; `AuthService signing key` → Auth = 2 + 1 + 1 = 4 ≥ 3, still first; the
`queue`/`cache` tie documents score 3 + 1 = 4 and keep their tie-break order; the service
fixture scores 6 + 1 + 3 + 3 = 13 > 0. Every assertion holds, so a 50% cut to the most
important weight would ship green.

**2 (medium) — pin the threshold.** *Goal:* write the smallest test that fails if
`minimumScore` stops defaulting to 3. *Hint:* you need a document whose only match is in
content. *Check:* `Search([new RetrievalDocument(id, "Runbook", "misc", "queue")], "queue")`
must be empty with the defaults (score 1 < 3). With the default lowered to 1 it returns one
match, so the test fails, which is what you want.

**3 (hard) — pin the window.** Do practice exercise 4 in [05-PRACTICE](05-PRACTICE.md) as
real test code against the existing `CreateDatabase` fixture. *Check:* your assertions are
`evidenceCount == 25` and `sampleLogIds.Length == 20`, and they use `detectedAt`-relative
timestamps far enough from the edges (e.g. ±1 hour) that the un-injectable
`DateTimeOffset.UtcNow` upper bound can't make the test flaky.

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
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
