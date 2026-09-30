# Review notes: ticket behavior and visibility

The transition helper permits same-state requests, blocks different-state moves out of Closed, restricts Resolved to Closed or InProgress and permits other pairs through its default branch. The service can still increment Version and add audit records for a same-state update. A stricter desired graph should be proposed separately from this characterization.

Invalid parsing falls back to current status at the service boundary, so malformed input may become a no-op state transition with side effects unless an earlier request validator rejects it. Numeric enum strings need actual .NET parsing tests. A successful parse is not automatically a defined named business value. Reopening retains the previous ResolvedAt under the inspected assignment, while later resolution or closure sets it again.

Ticket.Version is configured as a concurrency token, and relevant service paths increment it. A relational two-context test can examine conflict and rollback behavior. Model metadata alone does not establish API error translation, and EF InMemory should not be presented as a relational isolation test.

Object read access and field visibility are separate. The mapper includes comments with their IsInternal flag without filtering in the inspected function. Whether that is correct requires a product policy for internal notes. A learner should not infer confidentiality solely from the field name, but should build a role/relationship/field matrix and compare it with actual output.

Claims parsing, role capability, object relationship and projection are distinct boundaries. A manually constructed RequestContext does not test token validation. A SupportAnalyst can have mutation capability yet fail ticket ownership. Unauthorized tests should assert no comments, audits or other writes occur, not merely that a request returned an error.

[Index](../README.md)
