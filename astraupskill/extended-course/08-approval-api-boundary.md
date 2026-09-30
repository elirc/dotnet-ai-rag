# 8. A stored approval is not a complete approval workflow

[HumanApproval](../../src/Domain/Operations/HumanApproval.cs) stores recommendation type and JSON, status, requester, optional decision actor and timestamps. [ApprovalsController](../../src/Api/Controllers/ApprovalsController.cs) exposes a GET list protected by the CanInvestigate policy in the inspected source. The presence of decision-shaped fields does not prove a decision command or mitigation executor is implemented.

## Trace only what exists

The investigation service creates a Pending approval with the run's output JSON. The list service returns a bounded summary containing ID, recommendation type, status and creation time. The inspected controller has a list action. A learner should search for other decision paths before claiming approval, rejection or execution behavior. If none are found in the relevant source, describe those capabilities as proposed extensions.

This distinction matters because a UI label can imply a stronger workflow than storage supports. An operator may see Pending and assume clicking elsewhere will execute a mitigation. The course should teach the actual boundary: deterministic evidence and a recommendation record exist, while a complete decision-and-action lifecycle needs explicit commands, authorization and concurrency rules.

## Define a proposed decision command

A decision request could include approval ID, expected version, decision, rationale and the evidence or recommendation version being reviewed. The server derives actor identity from trusted request context, not from a caller-supplied decidedBy field. It validates authority, current Pending status and version before committing the decision and audit record together.

The version is important when two reviewers act concurrently. If one approves and another rejects from the same page state, the system needs a defined winner rule. A conditional transition from Pending can allow one decision and reject the stale second command. The losing reviewer should see the current decision and preserve their unsaved rationale for comparison rather than silently overwrite it.

## Decision and execution are separate

Approving a recommendation can mean accepting it as useful evidence, authorizing a future action or executing that action immediately. These meanings must not be conflated. For operational mitigation, a safer design often makes the action identity, target, parameters and preconditions explicit. This is a design exercise, not an installed execution system.

If an external action follows approval, its outcome can be uncertain after a timeout. The approval row should not be treated as proof that the action completed. An execution record with its own attempt identity and evidence may be needed. A local transaction cannot undo an external operational change merely because recording the result failed.

## Version the reviewed material

The recommendation JSON is copied from the run output at creation, which preserves that serialized recommendation. It still references knowledge documents whose content may change. A proposed review bundle should bind document versions or snapshots and the bounded log evidence. Otherwise a reviewer can approve text that later resolves to different instructions under the same document ID.

Design a stale-content test: open approval A, edit the referenced runbook, then submit the decision. Under a version-bound policy, the server should either preserve approval of the original snapshot or require a fresh review. State which policy you choose. Do not silently apply the click to new content.

## Exercise

Produce a current-capability map and a proposed decision state machine. Include Pending, decided, stale and unresolved-execution states only where their meaning is defined. Add tests for two reviewers, unauthorized actors, changed recommendation content and lost response after decision commit. The reviewer should be able to distinguish existing storage from future workflow guarantees at every step.

[Index](README.md) · [Review notes](answers/05-approval-outbox-and-json.md)
