# Review notes: approval, outbox and representation

The inspected approval controller exposes a list action. Decision-shaped entity fields do not establish a complete decision or execution workflow. A proposed command should bind actor authority, Pending status, expected version and reviewed recommendation content. Two competing reviewers need a conditional winner rule and a useful stale response.

Approval and execution are separate meanings. A decision can authorize an action without proving the action completed. A future external executor needs its own attempt identity and uncertain-outcome recovery. The current deterministic investigation does not execute a runbook simply because it creates a Pending approval.

The outbox worker currently logs IDs and types, marks selected messages Processed and saves. It does not call a delivery adapter. A two-worker claim or lease is not present in the inspected selection. Adding external effects inside the existing loop would create a gap before batch status save, so a future dispatcher needs identity, claim ownership and recovery rather than relying on the word outbox.

Correct JSON serialization preserves accepted strings with quotes, backslashes and control characters. Manual interpolation of PrimaryService can produce invalid representation for such values. Domain validation and encoding are separate. A serializer does not authorize a service name, and a valid service name still needs correct encoding.

For nullable assignment payloads, JSON null, empty string and missing property must have explicit meanings. A future consumer should validate version and shape and quarantine permanent malformed input rather than retry forever. The current worker's bookkeeping does not establish payload parsing or delivery correctness. A strong report names that limit beside the observed Processed status.

[Index](../README.md)
