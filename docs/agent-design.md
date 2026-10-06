# Agent Design

> **Implementation status (verified 2026-10-06):** the `IAgent`/`IAgentTool` abstractions below do not exist in `src/`. The one implemented "agent" is a deterministic method, `OperationsService.RunIncidentInvestigationAsync`. It runs two recorded tools (`SearchLogs`, `SearchKnowledge`), calls no model, saves `TotalEstimatedTokens = 0`, and creates a `Pending` `HumanApproval` that no endpoint can approve or reject yet. Ticket triage text (`AiSummary`) is a fixed template string, not an agent. See `astraupskill/01-CODEBASE-MAP.md`.

Agents begin in Phase 4. The key rule is that agents are backend services with tracked runs, not frontend-only chat logic.

## Core Abstractions

```csharp
public interface IAgent
{
    string Name { get; }
    Task<AgentRunResult> RunAsync(AgentRunRequest request, CancellationToken ct);
}

public interface IAgentTool
{
    string Name { get; }
    string Description { get; }
    Task<ToolResult> ExecuteAsync(ToolRequest request, CancellationToken ct);
}
```

## Safety Model

Agents can read logs, search docs, draft reports, suggest mitigations, and create timeline events. Risky actions require human approval, including closing incidents, finalizing root cause, creating external tickets, sending Slack updates, or changing deployment status.

## First Agents

- Ticket Triage Agent
- Incident Investigation Agent
- Postmortem Writer Agent
- Runbook Recommendation Agent
- Deployment Risk Agent

Every run will persist input, output, status, timing, estimated token use, estimated cost, and tool calls.
