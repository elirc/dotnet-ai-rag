# NetAiOps Upskill Lab

> **Status (2026-10-06):** this suite was written before the incident-investigation command
> was rebuilt around `LexicalRetriever` (see [`astraupskill/`](../../astraupskill/README.md),
> the current course for that workflow). Since then: the test suite has grown from the "1
> test" recorded below to **11 test cases** (7 `[Fact]` + one `[Theory]` × 4) across 4
> files, including retrieval and investigation-persistence tests; links into
> `OperationsService.cs` have been re-pointed to the current line numbers; and three method
> names that never existed in the code were corrected (`AddOutboxMessage` → `AddOutbox`,
> `CanTransition` → `IsAllowedTicketTransition`, `AddTimelineAsync` →
> `AddIncidentTimelineAsync`). The repo now has commits, so the "unborn branch" snapshot
> note below is historical.

Snapshot: `git rev-parse HEAD` could not produce a commit because this workspace is on an unborn Git branch with no commits. Fallback manifest hash for this documentation pass: `3b096fcb56b8df68733c39d8b9ac7346df0033384447fae750e7892a4d6f4106`. See [verification log](08-reference/verification-log.md).

## Repo Identity

NetAiOps is a freshly generated/synthetic C#/.NET and React training project, not an organically evolved OSS repository. Evidence: `git log --oneline -30` failed with `fatal: your current branch 'main' does not have any commits yet`; no CONTRIBUTING, CHANGELOG, SECURITY, or LICENSE file was found; and the implementation is a uniform Phase 1 vertical slice. The backend has layered .NET projects and wires ASP.NET Core, EF Core, JWT auth, BCrypt hashing, Swagger, CORS, and PostgreSQL composition in [Program](../../src/Api/Program.cs#L13-L111). The frontend is a Vite React app with React Router, TanStack Query, React Hook Form, Zod, and Zustand visible in [package.json](../../frontend/package.json#L6-L27). The implemented product surface is login, seeded users, protected dashboard, and placeholder routes for later modules. Treat modules 06 and 07 as a simulated OSS contribution workflow.

## Who This Is For

This suite is for a junior engineer growing toward mid-level and senior judgment. It teaches the current Phase 1 codebase and the transferable habits behind it: tracing runtime flow, respecting boundaries, testing behavior, debugging systematically, reviewing changes, and comparing architecture choices. It is intentionally grounded in files opened during this pass; claims without anchors are either omitted or labeled inferred.

Senior-mindset line: junior asks "how do I make it work?"; mid-level asks "is this the right pattern?"; senior asks "what does this commit us to, who pays, and how do we reduce risk?"

## How To Use It

| Track | Plan |
| --- | --- |
| Weekend | Start with [00-fast-track](00-fast-track.md), trace login and dashboard, then read one comparison card. |
| Two weeks | Add cartography, stack mental models, key flows, pattern catalog, and two good-first tickets. |
| Eight weeks | Work through debugging, security, performance, review katas, mid-level tickets, and interview prep. |
| Ongoing | Use risk register and architecture critique before each new phase. |

## Recommended Paths

| Learner | Path |
| --- | --- |
| Brand-new junior | `00-fast-track` -> `01-codebase-cartography/02-file-reading-order` -> `04-code-reading-gym/01-annotation-drills`. |
| Junior with stack familiarity | Key flows -> pattern catalog -> good-first tickets -> writing tests. |
| Mid-level new to repo | Architecture boundaries -> comparative implementations -> debugging -> mid-level feature tickets. |
| Senior doing architecture review | Verification log -> risk register -> architecture critique -> senior projects -> interview prep tradeoffs. |

## Conventions

Anchors use `SymbolName](relative/path#Lx-Ly)` and refer to the fallback manifest snapshot above. Invented snippets are labeled `Illustrative: not from this repo`. Drills include self-grading rubrics. Because this repo is synthetic, contribution exercises are role-played: you act as contributor, and the docs describe maintainer expectations.

