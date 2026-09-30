# 5. A retrieved candidate is not an authorized mitigation

The investigation method creates a completed AgentRun with two tool-call records, a pending HumanApproval and an incident candidate description. Its output explicitly records deterministic_retrieval, modelUsed false and modelConfidence null. The candidate asks a human to review the retrieved runbook against bounded logs. It does not establish a root cause or execute the runbook.

## Read the output contract literally

The two recorded tools are SearchLogs and SearchKnowledge. Their records include input and output JSON and phase timestamps. The run records zero estimated tokens and zero estimated cost in this path. These fields describe the inspected deterministic workflow. They should not be generalized to unrelated future agents or external model integrations.

If matches exist, the recommendation points to the top runbook and asks for validation. If none exist, the recommendation calls for manual investigation and warns against inferring a mitigation from absent evidence. The no-match path still creates the run and pending approval records in the inspected method. Therefore “completed” means the investigation workflow completed its retrieval work, not that the incident was solved.

## Preserve the chain of evidence

A useful review screen links incident identity, captured log bounds, sample identifiers, retrieval query, matched document identifiers, heuristic scores and matched terms. The reviewer should be able to inspect why a candidate appeared. A title alone is fragile because documents can be renamed. An identifier is stronger for identity but still does not preserve historical content if the document can later change.

A proposed provenance extension could store document version or content digest alongside the match. It should explain how the referenced content can be retrieved later. A digest without retained content detects a difference but does not reconstruct what the reviewer saw. Decide whether to snapshot the relevant excerpt, retain immutable document versions or store another durable reference. Label the choice as a proposal until implemented.

## Approval has its own lifecycle

Creating a Pending approval is not equivalent to receiving approval. Investigate the approval controller and service paths before describing who can decide it or what a decision does. This chapter intentionally does not invent those downstream semantics from the field name. The design exercise asks you to define a stronger contract for an operational action that could follow a review.

Such a contract should bind the decision to a specific recommendation version and evidence bundle. If a runbook changes after the reviewer opens the page, an approval should not silently authorize different steps. If the incident changes materially, the system may need a new review. These are product policy questions with concurrency implications, not merely UI checkbox behavior.

## Authorization before investigation

The method rejects callers who are not Admin or Engineer before loading the incident and creating evidence records. A test should verify that an unauthorized invocation creates no AgentRun, tool call, approval or audit mutation through this path. Checking only that an exception occurred can miss a side effect performed too early.

For an authorized call to a missing incident, the method returns null without constructing the investigation records. Distinguish unauthorized, missing and no-match outcomes. No-match is a valid completed investigation over an existing incident; it is not the same as a missing incident. A client should explain those states differently.

## Review exercise

Create an evidence-review worksheet with facts, hypotheses, proposed actions and authority in separate sections. Put a high lexical score in the facts section as a retrieval fact, not in the hypothesis section as proof of cause. Put a runbook mitigation in proposed actions until a person evaluates applicability and the action's approval contract is satisfied.

Submit a proposed version-bound approval request and two stale-decision scenarios. Include one document edit and one new investigation run. Your design passes if it preserves what was reviewed, identifies who may decide, and refuses to treat an old click as permission for changed content.

[Index](README.md) · [Answers](answers/03-evidence-and-approval.md)
