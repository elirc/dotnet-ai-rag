# ADR 0002: Direct EF Core In Infrastructure Services

## Status

Accepted.

## Context

The current service layer has simple query needs. EF Core already provides unit-of-work and query composition.

## Decision

Use `NetAiOpsDbContext` directly in infrastructure services. Avoid a generic repository layer until duplication or testing pressure justifies query objects.

## Consequences

- Fewer abstractions.
- Service tests may need EF test database support.
- Query shape remains visible and optimizable.
