# ADR 0003: Human Approval For Risky Agent Actions

## Status

Accepted.

## Context

Agents can draft reports and recommend mitigations, but production-like actions can have customer impact.

## Decision

Agents may read data and create draft recommendations automatically. Risky actions create `HumanApproval` records before execution.

## Consequences

- Safer AI workflow.
- Requires approval queue UX and audit trail.
- Evaluation harness should score unsafe-action avoidance.
