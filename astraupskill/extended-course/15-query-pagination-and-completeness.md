# 15. Bounded lists need honest completeness signals

SearchLogsAsync clamps page and pageSize, counts matching logs, then uses offset pagination ordered by descending timestamp. Ticket and incident lists also use count plus paged queries with their own orderings. Agent-run and approval lists simply take a fixed maximum. Each shape gives callers different information about completeness and stability.

## Offset pages over changing logs

Suppose page one contains the newest twenty-five logs. Before page two is requested, several newer logs arrive. The offset for page two is now measured against a shifted set, so rows can overlap or be skipped relative to the user's original view. This is a property of paging a changing collection, not necessarily a database bug.

Create a synthetic ordered list, take page one, insert two leading rows and take page two using the same offset rule. Write the exact repeated or missed IDs. Then compare a cursor based on timestamp and ID. A cursor can improve continuation behavior, but it still does not create an immutable snapshot unless the contract captures a stable observation boundary.

## Tie ordering

The bounded investigation sample adds an ID tie-break after timestamp. The general log search shown in the inspected method orders only by timestamp. Equal timestamps can therefore leave ordering dependent on provider behavior. Ticket and incident orderings also deserve a final unique key if deterministic page boundaries are required.

Design a relational test with many equal timestamps. Compare repeated pages and explain why one observed stable order is not proof of a total-order contract. A proposed ThenBy ID makes the intended tie rule explicit. Test the exact sequence and cursor behavior after adding it, including ascending or descending choices.

## Count and page consistency

CountAsync and the page query are separate reads. Concurrent inserts or deletes can make total and returned items describe slightly different populations under ordinary isolation. The UI should not treat the pair as a perfect snapshot without a stronger contract. For many browsing screens, approximate current totals are acceptable if the behavior is understood.

A compliance-style export or incident evidence capture may need stronger reproducibility. Define a snapshot boundary or retain selected identifiers. The investigation path already stores sample IDs, which is a useful example of preserving what was selected. A general list endpoint should not be repurposed as immutable evidence merely because it returns a total field.

## Fixed-limit lists

ListAgentRunsAsync takes one hundred and ListEvalRunsAsync takes fifty in the inspected source. ListApprovalsAsync takes one hundred. These methods do not return a next cursor in their current DTO shapes. A caller seeing one hundred items cannot infer that exactly one hundred exist. The UI should communicate “recent” or “latest” if that is the intended product boundary.

Propose a paged history endpoint only if the use case needs older records. It should preserve authorization and deterministic ordering. Adding pagination changes client expectations and may require a migration path for existing screens. Do not remove the useful bound simply to make all rows visible at once.

## Exercise

Submit a comparison table for paged logs, bounded investigation evidence and fixed-limit run lists. Include ordering keys, count semantics, completeness signals and appropriate use cases. A strong answer distinguishes browsing from evidence capture and avoids interpreting an empty page or truncated list as proof of global absence.

[Index](README.md) · [Review notes](answers/07-query-and-runtime.md)
