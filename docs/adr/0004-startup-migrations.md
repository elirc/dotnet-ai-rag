# ADR 0004: Startup Migrations Are Development Only

## Status

Accepted with constraint.

## Context

Running migrations at API startup makes local development easy but can be unsafe in multi-instance deployments.

## Decision

Keep `SeedDatabaseOnStartup` for local training. Production-like environments should run migrations as a separate release step or job.

## Consequences

- Local setup remains simple.
- Deployment docs must warn against automatic multi-instance migration.
- CI should validate migrations compile.
