# NetAiOps

NetAiOps is a realistic AI-powered IT operations platform built as a phased learning project. The finished system will include enterprise CRUD workflows, RAG over internal runbooks, autonomous investigation agents, MCP-style mock integrations, and an evaluation harness.

## Current Phase

Phase 1 foundation is implemented:

- ASP.NET Core 8 API with layered projects under `src/`
- EF Core + PostgreSQL wiring and an initial identity migration
- JWT login with seeded users and role-based policies
- Protected dashboard summary endpoint
- React + TypeScript + Vite frontend with login, protected routes, dashboard shell, and future module placeholders
- Unit test for password hashing
- Docker Compose for local PostgreSQL

Production-realism backbone now includes:

- Ticket, incident, timeline, comments, audit, logs, knowledge documents, agent runs, eval runs, approvals, settings, and outbox tables
- Database-backed dashboard metrics
- Bounded list APIs with RBAC/resource-aware checks
- Correlation IDs, security headers, global problem responses, rate-limited login, and health checks
- Seeded operational scenarios for BillingService and AuthService
- Real frontend pages for tickets, incidents, logs, knowledge base, agents, evals, users, and settings
- CI workflow, PR/issue templates, CODEOWNERS, ADRs, Dockerfiles, and security/contribution docs

Seed accounts all use `Password123!`:

- `admin@example.com` / Admin
- `engineer@example.com` / Engineer
- `support@example.com` / SupportAnalyst
- `viewer@example.com` / Viewer

## Local Development

```powershell
docker compose up -d postgres
& 'C:\Users\Owner\.dotnet\dotnet.exe' restore
& 'C:\Users\Owner\.dotnet\dotnet.exe' run --project src/Api
```

In another terminal:

```powershell
cd frontend
npm install
npm run dev
```

Open `http://localhost:5173`.

Optional full Docker profile:

```powershell
docker compose --profile app up --build
```

This starts Postgres, API on `http://localhost:8080`, and static frontend on `http://localhost:8081`.

## Verification

```powershell
& 'C:\Users\Owner\.dotnet\dotnet.exe' build NetAiOps.sln
& 'C:\Users\Owner\.dotnet\dotnet.exe' test NetAiOps.sln
cd frontend
npm run build
```

## Documentation

- [Architecture](docs/architecture.md)
- [Database Schema](docs/database-schema.md)
- [Implementation Roadmap](docs/implementation-roadmap.md)
- [Local Development](docs/local-development.md)
- [RAG Design](docs/rag-design.md)
- [Agent Design](docs/agent-design.md)
- [MCP Mock Integrations](docs/mcp-mock-integrations.md)
- [Evaluation Harness](docs/eval-harness.md)
- [Phase Journals](docs/phases)

## Source-based improvement course

[Start the course](astraupskill/README.md). Trace deterministic incident retrieval, truthful evidence, and pending human approval.
