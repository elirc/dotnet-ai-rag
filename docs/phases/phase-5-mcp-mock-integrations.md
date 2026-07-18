# Phase 5 Journal: MCP Mock Integrations

## Planning

Phase 5 simulates external enterprise tools. The teaching goal is designing integrations as capabilities with observability, not scattered HTTP calls.

## Implementation Plan

- Add mock GitHub, Jira, Slack, and Database MCP servers.
- Add a common MCP call service.
- Persist every MCP call.
- Make selected agent tools call through this layer.

## Retrospective Template

After implementation, document how mock servers differ from real MCP transport and what parts of the design can remain stable when real integrations are added.
