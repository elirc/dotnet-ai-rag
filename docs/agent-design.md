# Agent Design

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
