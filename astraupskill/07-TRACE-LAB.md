# Trace one incident into its saved records

Two traces: first the controlled test fixture (every answer is pinned by an assertion you
can point at), then the seeded demo data (every answer is a prediction you derive from
source). Fill in each table *before* reading the Check.

## Trace A: the test fixture

Use the seeded billing incident in
[`OperationsServiceTests.cs:17-67`](../tests/UnitTests/OperationsServiceTests.cs#L17-L67).
Follow the command's role check before drawing any arrows to persistence. The request
context is part of the input contract even though it isn't a search term.

| Step | What to record | Your value |
| --- | --- | --- |
| 1 | caller role, and the line that lets it through | |
| 2 | `logWindowStart` in terms of `detectedAt` | |
| 3 | does the one seeded log fall inside the window? why? | |
| 4 | `evidenceCount`, `sampleLogIds` | |
| 5 | retrieval query terms (chapter 2) and the runbook's score | |
| 6 | contents of the `SearchLogs` and `SearchKnowledge` tool rows | |
| 7 | approval status; incident candidate text; audit event type | |

**Check:**
1. `Engineer` passes `IsAdminOrEngineer`
   ([`RequestContext.cs:16`](../src/Application/Common/RequestContext.cs#L16)).
2. `detectedAt - 2h`, asserted at test line 84.
3. Yes: its timestamp is `detectedAt + 1 min` and its `ServiceName` equals the incident's
   `PrimaryService` exactly, odd characters included.
4. `1` (asserted at line 91) and `[logId]` (line 98).
5. 17, matched terms `billing, payment, capture, payments`. Derived in chapter 2; the test
   only asserts `> 0` (line 94).
6. `SearchLogs` → `matchingLogs: 1`, `sampleLogIds: [logId]`; `SearchKnowledge` →
   one match with `documentId`, asserted at lines 99-102.
7. `Pending` (line 105); the candidate begins `"Deterministic retrieval candidate: review
   Billing capture timeout runbook"` ([`OperationsService.cs:429`](../src/Infrastructure/Operations/OperationsService.cs#L429),
   *not* asserted by any test); `AgentRunCreated` (line 107).

Now compare the parent `OutputJson` with the two children. A parent summary that names a
document absent from the saved `SearchKnowledge` evidence would be a consistency defect.
Here both are built from the same `matches` list (lines 363-369 and 406-412), which is why
they agree. That's a structural guarantee, not just a tested one.

## Trace B: the seeded demo data (prediction)

The seeder creates a billing incident
([`DatabaseSeeder.cs:55-72`](../src/Infrastructure/Persistence/DatabaseSeeder.cs#L55-L72)),
80 synthetic logs ([lines 309-340](../src/Infrastructure/Persistence/DatabaseSeeder.cs#L309-L340)),
and four knowledge documents
([lines 139-143](../src/Infrastructure/Persistence/DatabaseSeeder.cs#L139-L143)), all from
one `now` (line 20). Predict what `POST /api/incidents/{billing id}/run-investigation`
saves on a freshly seeded database.

*Hints:* logs rotate through five services by `i % 5`, so `BillingService` is every fifth
`i`; log `i` is stamped `now - 6i` minutes. `DetectedAt` is `now - 2.5h`, so the lower bound
is `now - 4.5h` = `now - 270 min`. For retrieval, tokenize the incident's service, title,
description, and affected-services CSV, and remember `with` is a stop word but `is` isn't.

**Check (derived from source, not from a run):**
- **`evidenceCount = 10`**: `i ∈ {0, 5, …, 45}`. The tenth log (`i = 45`, exactly
  `now - 270 min`) sits *on* the lower bound and is included only because the comparison is
  `>=` ([`OperationsService.cs:323`](../src/Infrastructure/Operations/OperationsService.cs#L323)).
  Change it to `>` and the count becomes 9. Boundary operators are behavior. The count stays
  10 however long after seeding you run it, because the window's start is anchored to the
  stored `DetectedAt`, not to "now".
- **One match, `BillingService Timeout Runbook`, score 17**: `billingservice` 5 (title +
  content), `timeout` 7 (title + tags; content says `timeouts`), and 1 each for `spike`,
  `checkout`, `database`, `connection`, `pool`. The other three documents score 0.
  `saturated` vs `saturation` and `timeout` vs `timeouts` are both missed, the same
  no-stemming lesson as chapter 2.
- The seeder already set `AiRootCauseCandidate` to a hand-written string ("Likely database
  connection pool exhaustion…"). The investigation **overwrites** it (line 428) with the
  deterministic-retrieval text. Is losing the earlier candidate acceptable? That's a
  product question worth raising (extended lesson 14 discusses "latest save wins").

If you have the stack running, confirm by calling the endpoint as `engineer@example.com` and
reading the new `agent_runs` row. If your numbers differ, find out which hint you
misapplied before you suspect the code.

## Finish at the approval

Neither trace applies a mitigation. Record this distinction: retrieval completed, a
recommendation exists, and execution has **not** been approved. Nothing in the API *can*
approve it yet (chapter 1). Repeat Trace A mentally with unrelated knowledge (a truthful
no-match investigation, practice exercise 3) and with a Viewer (exit before any row,
practice exercise 5). Those three traces make the workflow's success, uncertainty, and
rejection paths concrete.

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
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
