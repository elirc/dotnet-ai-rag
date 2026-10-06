# From invented metadata to saved evidence

This chapter studies a real change. The pre-change method is preserved in
[`snapshots/src__Infrastructure__Operations__OperationsService.cs.original.txt`](snapshots/src__Infrastructure__Operations__OperationsService.cs.original.txt);
the current one is
[`OperationsService.RunIncidentInvestigationAsync`](../src/Infrastructure/Operations/OperationsService.cs#L305-L437).
Diff them yourself (`git diff --no-index` works on any two files) before reading the table.

## Before → after, field by field

| Saved value | Before (snapshot) | After (current) | Why it matters |
| --- | --- | --- | --- |
| `confidence` in output | hard-coded `0.72` | `modelConfidence: null`, `modelUsed: false` (lines 360-361) | a number nobody computed looked like model output |
| `TotalToolCalls` | `3`, with a single tool-call row | `2`, with exactly two rows (lines 383, 387-417) | the count must describe the children that exist |
| `TotalEstimatedTokens` / `EstimatedCost` | `1800` / `0.012m` | `0` / `0m` (lines 384-385) | no provider was called, so no tokens were spent |
| `CompletedAt` | `now.AddSeconds(8)` | measured `DateTimeOffset.UtcNow` (line 346) | an invented duration is a fabricated measurement |
| Log window | lower bound only | lower **and** upper bound, saved in input JSON (lines 318-324, 350-356) | evidence must name its own scope |
| Log evidence | a count | count **plus** up to 20 sample ids (lines 325-330, 393) | a reviewer can open the actual rows |
| Knowledge evidence | none (a canned "follow linked runbook" sentence) | `LexicalRetriever` over stored documents, saved as `SearchKnowledge` (lines 341-344, 398-417) | the recommendation now names a document that was actually retrieved |
| JSON construction | `$$"""{ "service":"{{...}}" }"""` string interpolation | `JsonSerializer.Serialize` (lines 350-371, 392-393, 403-413) | a service name containing `"` or `\` produced invalid JSON before |

## Read the current method as four phases

1. **Authorize and load** (lines 307-316). Viewer → exception; missing incident → `null`.
2. **Gather bounded evidence** (lines 318-345). The log interval begins two hours before
   detection and ends at the command's captured start time; the count and the id sample
   share one predicate (`logQuery`). Retrieval combines primary service, title,
   description, and affected services into one query string.
3. **Describe** (lines 347-371). With no match, the recommendation explicitly asks for
   manual investigation, and the no-match path does not invent a mitigation.
4. **Persist** (lines 372-434). Run, two tool calls, pending approval, incident candidate,
   audit event: one `SaveChangesAsync`.

Keeping those phases visible helps distinguish a failed search from a failed save.

## What the change did *not* fix (senior-reviewer flags)

Each of these is verifiable in the current file, and a good reviewer would mention them in
the PR rather than let them pass silently:

- **The same JSON pattern survives elsewhere.** `AddOutbox` payloads are still built by
  string interpolation: `CreateTicketAsync` (line 116), `AssignTicketAsync` (line 164), and
  `CreateIncidentAsync` (line 268). The first two only interpolate GUIDs and enum names, so
  they happen to stay valid. Line 268 interpolates `PrimaryService`, which is free text from
  the request: an incident created with a `"` in its service name writes invalid JSON into
  `OutboxMessages`. "Safe today because of the current inputs" is still a pattern worth
  flagging. (Extended lesson 10 develops this.)
- **The whole corpus is loaded per investigation** (lines 341-343). `topK: 3` bounds the
  output, not the scan. Fine at four seeded documents; linear in corpus size forever.
- **The incident update doesn't advance `Version`.** `Incident.Version` is a concurrency
  token ([`NetAiOpsDbContext.cs:135`](../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L135)),
  but lines 428-432 change the incident without `incident.Version++` (compare
  `AddIncidentTimelineAsync`, line 299). Two concurrent investigations therefore both
  succeed and the later save's candidate text wins. (Extended lesson 14.)
- **The approval is an orphan.** `HumanApproval` has no `AgentRunId` or `IncidentId`
  ([`HumanApproval.cs`](../src/Domain/Operations/HumanApproval.cs)). The only link is the
  copied `RecommendationJson`, and no endpoint can decide it. (Extended lessons 8 and 19.)
- **Time is not injectable.** `DateTimeOffset.UtcNow` is called directly (lines 318,
  331-333, 345-346), so tests can only assert orderings (`BeOnOrAfter`), not exact
  timestamps. Chapter 6 returns to this.

## Exercise: review the diff like a maintainer

*Goal:* produce the review comment a senior would leave on this change.
*Hint:* read the snapshot's version of the method end to end first, and write down every
value in the saved rows that the old code *invented*.
**Check:** your list should include at least the five fabricated values in the table
(confidence, tool count, tokens, cost, completion time), and your review should mention at
least two of the "did not fix" flags above with their line numbers. If you only found the
confidence number, re-read the `AgentRun` initializer in the snapshot: every numeric field
in it was a constant.

## Source excerpt

From [src/Infrastructure/Operations/OperationsService.cs](../src/Infrastructure/Operations/OperationsService.cs).

```cs
        var outputJson = JsonSerializer.Serialize(new
        {
            mode = "deterministic_retrieval",
            modelUsed = false,
            modelConfidence = (double?)null,
            evidenceCount,
            runbookMatches = matches.Select(match => new
            {
                documentId = match.DocumentId,
                match.Title,
                heuristicScore = match.HeuristicScore,
                match.MatchedTerms
            }),
            recommendedMitigation = recommendation
        });
        var agentRun = new AgentRun
        {
            Id = Guid.NewGuid(),
            AgentName = "IncidentInvestigationAgent",
            InputJson = inputJson,
            OutputJson = outputJson,
            Status = "Completed",
            StartedAt = startedAt,
            CompletedAt = completedAt,
            CreatedByUserId = context.UserId,
            RelatedIncidentId = incident.Id,
            TotalToolCalls = 2,
            TotalEstimatedTokens = 0,
            EstimatedCost = 0m
        };
        agentRun.ToolCalls.Add(new Domain.Agents.AgentToolCall
        {
            Id = Guid.NewGuid(),
            AgentRunId = agentRun.Id,
            ToolName = "SearchLogs",
            InputJson = JsonSerializer.Serialize(new { service = incident.PrimaryService, logWindowStart, logWindowEnd }),
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
