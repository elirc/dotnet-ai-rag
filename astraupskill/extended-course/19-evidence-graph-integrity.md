# 19. A JSON identifier is not necessarily a relational link

The investigation stores structured entities and JSON payloads. AgentToolCall has an AgentRunId relationship configured in the DbContext. HumanApproval stores RecommendationJson. Run output includes document IDs and log sample IDs inside serialized data. These references do not all receive the same database enforcement or query behavior.

## Map the graph

Draw the incident, agent run, two tool calls, approval, audit event, knowledge documents and logs. Use one edge style for configured relational foreign keys and another for identifiers embedded in JSON. A JSON documentId can preserve association information without the database enforcing that the referenced document still exists. That may be intentional for historical evidence, but it should be understood.

The approval's recommendation JSON copies the run output, but the inspected HumanApproval entity does not show an AgentRunId property. A future feature that needs direct navigation from approval to exact run may require an explicit link. Do not infer one merely because the serialized content looks similar. Search and schema inspection should establish the actual relationship.

## Deletion and retention

Configured cascade deletion can remove tool calls when an agent run is deleted under the relationship model. Embedded IDs can remain in JSON after their source documents or logs are removed. The resulting evidence may be partially resolvable. Define a retention policy that explains whether historical investigations keep snapshots, tombstone references or only identifiers.

A tombstone can preserve identity and deletion reason without retaining all content. A snapshot can preserve reproducibility but may contain sensitive information requiring restricted access. A live reference can be compact but changes when the source changes. There is no universal best choice; the course asks you to connect the policy to the incident-review use case.

## Consistency checks

Build a read-only integrity checker for synthetic evidence graphs. Verify every tool call belongs to the intended run, TotalToolCalls matches the recorded collection under the chosen definition, output document IDs correspond to the match list, and approval recommendation content matches the intended run version. If some links are deliberately historical and unresolved, report that category rather than treating it as an ordinary foreign-key failure.

The checker should not silently repair data. It should produce a reviewable report with IDs, categories and proposed next observations. A missing source document may require a retained snapshot or a notice, not deletion of the entire investigation. Preserve evidence before deciding a repair.

## Schema evolution

If output JSON gains new fields or changes shape, old runs remain stored. A version field can let readers select the appropriate parser. Without one, readers may need tolerant parsing and careful defaults. A default such as modelUsed true would be especially misleading for old deterministic runs. Unknown metadata should remain unknown rather than being filled with a stronger claim.

Create two synthetic payload versions and a reader contract. Define required fields, optional fields and unknown-version behavior. Include a malformed historical record and decide whether the UI shows a bounded error with raw-data access for authorized operators. Do not let one bad row break the entire recent-run list.

## Deliverable

Submit the relational/JSON graph, retention choices, integrity-check report and payload-version plan. A strong answer distinguishes stored identity from enforced referential integrity and preserves the ability to explain old investigations after source data evolves.

[Index](README.md) · [Review notes](answers/09-cancellation-and-graphs.md)
