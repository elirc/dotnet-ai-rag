# NetAiOps Upskill Lab 2.0

Snapshot: `git rev-parse HEAD` could not produce a commit because this workspace is on an unborn Git branch with no commits. `git log --oneline -30` also failed with `fatal: your current branch 'main' does not have any commits yet`. Fallback manifest hash for this documentation pass: `ee26a296a11a8f245385f609e37d761721a64bee674be0152b11abda35e27c87` across 181 tracked workspace files, excluding build outputs and this `docs2.0` folder. See [verification log](08-reference/verification-log.md).

## Repo Identity

NetAiOps is a synthetic enterprise-style C#/.NET and React training project that now models a mid-size-company internal operations platform. The backend composes controllers, authentication, EF Core persistence, policies, middleware, health checks, and seeded operational scenarios in [Program](../src/Api/Program.cs#L15-L137). The core product behavior is concentrated in [OperationsService](../src/Infrastructure/Operations/OperationsService.cs#L12-L493), with durable tables configured in [NetAiOpsDbContext](../src/Infrastructure/Persistence/NetAiOpsDbContext.cs#L13-L300). The frontend has real routed workspaces for dashboard, tickets, incidents, logs, knowledge, agents, evals, users, and settings in [App](../frontend/src/app/App.tsx#L16-L43), not only placeholders.

This repo still has no organic Git history, so modules 06 and 07 are written as a simulated company contribution workflow. Treat the exercises as if you were joining a team with code review, CI, production risk, and operational ownership.

## Who This Is For

This suite is for a junior engineer growing into mid-level production judgment. It teaches how to trace request flow, read domain models, understand persistence and authorization, evaluate architecture tradeoffs, write focused tests, and communicate changes like a professional teammate.

Senior-mindset line: junior asks "how do I make it work?"; mid-level asks "is this the right pattern?"; senior asks "what does this commit us to, who pays, and how do we reduce risk?"

## How To Use It

| Track | Plan |
| --- | --- |
| Weekend | Start with [00-fast-track](00-fast-track.md), trace tickets and incident investigation, then read the risk register. |
| Two weeks | Add cartography, stack mental models, key flows, pattern catalog, and two good-first tickets. |
| Eight weeks | Work through debugging, security, performance, review katas, mid-level tickets, and interview prep. |
| Ongoing | Use architecture critique and verification log before extending any feature. |

## Modules

- [00 Fast Track](00-fast-track.md)
- [01 Codebase Cartography](01-codebase-cartography/README.md)
- [02 Stack And Language Mastery](02-stack-and-language-mastery/README.md)
- [03 Architecture And Patterns](03-architecture-and-patterns/README.md)
- [04 Code Reading Gym](04-code-reading-gym/README.md)
- [05 Quality Engineering](05-quality-engineering/README.md)
- [06 Contribution Practice](06-contribution-practice/README.md)
- [07 Career And Collaboration](07-career-and-collaboration/README.md)
- [08 Reference](08-reference/verification-log.md)
- [Changes Since Docs 1](changes-since-docs1.md)

## Conventions

Anchors use relative links with line ranges and only cite files opened during this pass. Claims based on commands are marked verified. Claims based on reading code are tied to source anchors. Where a recommendation goes beyond current implementation, it is labeled as a proposed exercise or inferred risk.
