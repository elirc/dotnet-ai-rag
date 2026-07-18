# Database Schema

This document tracks the planned schema by phase. Phase 1 is implemented in the `InitialIdentity` EF Core migration.

## Phase 1 Implemented Tables

### `users`

| Column | Type | Notes |
| --- | --- | --- |
| `id` | `uuid` | Primary key |
| `email` | `varchar(256)` | Required, unique |
| `display_name` | `varchar(160)` | Required |
| `password_hash` | `varchar(512)` | BCrypt hash |
| `role` | `varchar(64)` | `Admin`, `Engineer`, `SupportAnalyst`, `Viewer` |
| `is_active` | `boolean` | Used to disable logins |
| `created_at` | `timestamp with time zone` | UTC |
| `updated_at` | `timestamp with time zone` | UTC |

## Planned Phase 2 Tables

- `tickets`
- `ticket_comments`
- `ticket_internal_notes`
- `ticket_audit_events`
- `incidents`
- `incident_timeline_events`
- `incident_tickets`
- `logs`
- `incident_logs`

## Planned Phase 3 Tables

- `knowledge_documents`
- `document_chunks`
- `rag_queries`
- `rag_citations`

## Planned Phase 4 Tables

- `agent_runs`
- `agent_tool_calls`
- `agent_recommendations`
- `human_approvals`

## Planned Phase 5 Tables

- `mcp_calls`
- `mcp_server_settings`

## Planned Phase 6 Tables

- `eval_cases`
- `eval_runs`
- `eval_results`

## Design Notes For Junior Engineers

Start with the workflows, then model data. Tickets and incidents look similar at first, but they answer different business questions: tickets represent support work, while incidents represent coordinated production response. Keeping those tables distinct avoids tangled status logic later.
