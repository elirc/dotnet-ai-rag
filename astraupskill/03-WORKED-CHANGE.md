# From invented metadata to saved evidence

Read the command as four phases. First, authorize and load the incident. Second, gather evidence with explicit bounds. Third, describe the evidence and proposed next step. Fourth, persist the run and approval. Keeping those phases visible helps distinguish a failed search from a failed save.

The log interval begins two hours before detection and ends at the command's captured start time. Both the count and identifier sample use this same predicate. Retrieval combines the primary service, title, description, and affected services into a query. The knowledge projection supplies the fields needed by the pure ranking function. Three matches is the current output bound; the knowledge corpus itself is loaded in memory, so this implementation does not claim bounded database scanning for a large corpus.

JsonSerializer constructs the input and output documents. Service names containing quotation marks or backslashes therefore remain valid JSON. SearchKnowledge saves the selected document identifiers, scores, and terms. SearchLogs saves its actual interval and sample identifiers. TotalToolCalls is two because precisely those two child records are created.

When no runbook passes the threshold, the recommendation explicitly requests manual investigation. When a match exists, it asks a reviewer to validate the runbook against logs. Both paths create a pending approval. Zero model tokens and zero model cost describe this command's actual provider usage; they do not mean the database or hosting is free. The incident candidate and audit summary use the same deterministic-retrieval description.

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
