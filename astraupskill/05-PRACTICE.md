# Exercises in ranking and evidence inspection

Work on paper first. Every exercise has a **Check** you can confirm by reading the cited
code or test. Answers and review criteria are in
[06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md). Open them only after you've written
your own.

None of these needs a provider credential, a deployed incident, or an operational
database. Keep any fixture changes local and isolated, the way `CreateDatabase` already
does ([`OperationsServiceTests.cs:140-146`](../tests/UnitTests/OperationsServiceTests.cs#L140-L146)).

## 1. Scores and order (easy)

*Goal:* apply weights, threshold, and `topK` exactly.
Query `queue`. Document A has `queue` in its title and content, B only in tags, C only in
content. Use the production weights and threshold. State the returned order, say which
document is excluded and why, then repeat with `topK: 1`. Write the expected identifiers,
not just the number of results.
*Hint:* [`LexicalRetriever.cs:44-46`](../src/Application/Retrieval/LexicalRetriever.cs#L44-L46) and line 27.
**Check:** your numbers must be consistent with the existing test at
[`LexicalRetrieverTests.cs:30-42`](../tests/UnitTests/LexicalRetrieverTests.cs#L30-L42),
where two documents with `queue` in tags + content (3 + 1 = 4) both pass, and `topK: 1`
keeps the one whose title sorts first.

## 2. Ties and input order (easy)

*Goal:* explain why the result doesn't depend on input order.
Give two documents the same score and title but different identifiers. Reverse their input
order. Predict the output *before* reading the tie-break expression. Explain why a stable
sort alone would not give the same guarantee when the source enumeration changes.
**Check:** the second half of the same test (lines 40-41) passes `laterId` first and
expects `earlierId` first in the output. Your explanation must name line 30 of
`LexicalRetriever.cs`.

## 3. The truthful no-match investigation (medium)

*Goal:* know exactly what gets saved when nothing matches.
Design an investigation with no relevant knowledge document. List every persisted record
and the output values you expect. Pay attention to `modelUsed`, `modelConfidence`, tool
count, `runbookMatches`, approval status, the recommendation text, and the incident's
`AiRootCauseCandidate`.
**Check:** trace [`OperationsService.cs:344-434`](../src/Infrastructure/Operations/OperationsService.cs#L344-L434)
with `matches.Count == 0`. Every string you predict must appear verbatim in lines 349 and
430. Absence of a runbook is a valid search outcome; it is not permission to invent a
mitigation.

## 4. The log window, with edges (medium)

*Goal:* separate "how many matched" from "how many we kept".
Propose a service test containing twenty-five matching logs, one log before the lower bound,
and one log after the upper bound. Predict the total count and the sample size separately.
Specify how you'd make the time boundaries deterministic without a sleep.
*Hint:* lines 318-330. The upper bound is `startedAt`, the moment the command ran.
**Check:** `evidenceCount` and `sampleLogIds` come from the *same* `logQuery` with
different terminal operators (`CountAsync` vs `Take(20)`), so your two numbers must differ.
Your "after the upper bound" log must have a timestamp in the *future* relative to test
execution, because there is no injectable clock.

## 5. What can a Viewer change? (medium)

*Goal:* reason about authorization at two layers.
Describe what an unauthorized Viewer request to `POST /api/incidents/{id}/run-investigation`
may change, first via HTTP, then by calling the service directly.
**Check:** compare with
[`IncidentsController.cs:43`](../src/Api/Controllers/IncidentsController.cs#L43) (policy)
and the existing test
[`Investigation_rejects_viewer_without_persisting_run_or_approval`](../tests/UnitTests/OperationsServiceTests.cs#L110-L138).

## 6. Who reads the internal notes? (hard)

*Goal:* see that "can read the object" and "can read every field" are different policies.
A SupportAnalyst creates a ticket and assigns it to a user whose role is `Viewer`, then adds
a comment with `IsInternal = true`. The Viewer calls `GET /api/tickets/{id}`. What do they
receive?
*Hint:* follow `GetTicketAsync` → `CanReadTicket` → `ToTicketDetail`
([`OperationsService.cs:62-76`](../src/Infrastructure/Operations/OperationsService.cs#L62-L76),
[517-520](../src/Infrastructure/Operations/OperationsService.cs#L517-L520),
[584-603](../src/Infrastructure/Operations/OperationsService.cs#L584-L603)).
**Check:** your answer must say whether line 601 filters on `IsInternal`, and whether any
test covers this path. (Search `tests/` for `IsInternal`.)

## 7. Two investigations at once (hard)

*Goal:* predict a concurrency outcome from configuration, not intuition.
Two Engineers start an investigation on the same incident within the same second. What rows
exist afterwards, and whose `AiRootCauseCandidate` survives? Now repeat with one
investigation and one `AddIncidentTimelineAsync` call racing.
*Hint:* `Incident.Version` is a concurrency token
([`NetAiOpsDbContext.cs:135`](../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L135)).
Which of the two methods *changes* it?
**Check:** compare line 299 (`incident.Version++`) with lines 428-432 (no increment). Your
two answers must differ, and the second must name the HTTP status produced by
[`ExceptionHandlingMiddleware.cs:20`](../src/Api/Middleware/ExceptionHandlingMiddleware.cs#L20).
This is a prediction from source; the InMemory fixture can't confirm it (chapter 4).
Extended lesson 14 designs the relational test that would.

## Source excerpt

From [tests/UnitTests/LexicalRetrieverTests.cs](../tests/UnitTests/LexicalRetrieverTests.cs).

```cs
    [Fact]
    public void Ranking_is_stable_bounded_and_uses_title_then_id_for_ties()
    {
        var b = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000011"), "B Runbook", "queue", "queue");
        var a = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000012"), "A Runbook", "queue", "queue");
        var laterId = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000014"), "Same Runbook", "cache", "cache");
        var earlierId = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000013"), "Same Runbook", "cache", "cache");
        var results = LexicalRetriever.Search([b, a, Billing], "queue", topK: 1);
        results.Should().ContainSingle();
        results[0].Title.Should().Be("A Runbook");
        LexicalRetriever.Search([laterId, earlierId], "cache")
            .Select(x => x.DocumentId).Should().Equal(earlierId.Id, laterId.Id);
    }

    [Fact]
    public void Unrelated_or_stop_word_only_queries_return_no_match()
    {
        LexicalRetriever.Search([Billing, Auth], "printer toner tray").Should().BeEmpty();
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
