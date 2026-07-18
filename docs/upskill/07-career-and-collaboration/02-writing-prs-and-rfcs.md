# Writing PRs And RFCs

## PR Template

- What changed
- Why it changed
- How tested
- Risk and rollback
- Follow-ups

## Example PR Summary

```text
Adds JWT options validation so invalid issuer/audience/signing-key config fails at startup.
Tested with dotnet test NetAiOps.sln.
Risk: startup may now fail in misconfigured dev environments; rollback by reverting validation registration.
```

## RFC Template

- Problem
- Current behavior with anchors
- Options considered
- Decision
- Migration plan
- Test plan
- Security/performance impact
- Rollback

Use [Comparative Implementations](../03-architecture-and-patterns/07-comparative-implementations.md) as the source for options.

