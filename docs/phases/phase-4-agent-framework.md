# Phase 4 Journal: Agent Framework

## Planning

Phase 4 introduces autonomous workflows. The teaching goal is to make agent behavior inspectable: every run and tool call should be persisted and reviewable.

## Implementation Plan

- Add `IAgent` and `IAgentTool`.
- Add `agent_runs` and `agent_tool_calls`.
- Implement ticket triage agent.
- Implement incident investigation agent.
- Add agent run pages.
- Add safety gates for risky recommendations.

## Retrospective Template

After implementation, document how the agent workflow is bounded, which tools are read-only, and what requires human approval.
