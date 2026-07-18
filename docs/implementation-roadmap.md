# Implementation Roadmap

## Phase 1: Foundation

Implemented now.

- Solution structure
- React app
- ASP.NET Core API
- PostgreSQL and EF Core migration
- Auth and RBAC
- Seed users
- Dashboard shell

## Phase 2: Core IT Ops CRUD

- Tickets, comments, internal notes, assignment, status transitions, and audit trail
- Incidents, linked tickets, linked logs, timeline events, and affected services
- Synthetic logs with filtering and trace grouping

## Phase 3: Knowledge Base And RAG

- Document CRUD and uploads
- Chunking pipeline
- Mock embedding provider
- Hybrid retrieval
- RAG assistant endpoint and UI with citations

## Phase 4: Agent Framework

- Agent and tool abstractions
- Agent run and tool-call persistence
- Ticket triage agent
- Incident investigation agent

## Phase 5: MCP Mock Integrations

- GitHub, Jira, Slack, and Database MCP-style mock servers
- MCP call logging
- Agent-safe tool adapter layer

## Phase 6: Evaluation Harness

- Eval cases and suites
- Retrieval evals
- RAG answer evals
- Agent workflow evals
- Regression run comparison

## Phase 7: Polish And Realism

- Rich seed scenarios
- Charts and operational UX polish
- Loading, empty, and error states everywhere
- Broader unit and integration test coverage
- Final README and architecture documentation pass
