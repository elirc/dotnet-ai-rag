# 27. Design an investigation that can be reconstructed later

This capstone proposes a versioned evidence bundle linking one investigation command to its incident input, log selection, knowledge content, retrieval algorithm and human review. The current application already stores useful JSON and identifiers. The exercise extends that foundation without pretending every historical record can be made perfectly reproducible after the fact.

## Define the reconstruction question

Do you need to reproduce the exact ranking, explain what the operator saw, or verify that an approved action matched reviewed content? These questions overlap but require different retained data. Exact ranking needs corpus or index version and algorithm parameters. Operator explanation needs displayed excerpts and metadata. Action authorization needs a version-bound structured proposal and decision record.

Choose one primary question and state secondary goals. Avoid storing every log and document indefinitely merely because it simplifies the exercise. Retention, access and storage cost are real design constraints. The bundle should preserve enough evidence for its purpose and explain what remains unavailable.

## Proposed bundle fields

Include command ID, run ID, incident version or snapshot, captured time bounds, selected log IDs or snapshots, corpus/index version, matched document versions, algorithm version, parameters, output mode and reviewer decision references. Some fields can be references to immutable records rather than copied content. The choice should make resolution reliable under the retention policy.

An algorithm version should identify behavior, not only a display name. A code commit or content hash can help reproduce the implementation, but runtime and configuration also matter. A corpus version must identify the content actually searched, including eligibility filters. Recording only the latest document IDs is insufficient when content changes.

## Atomicity and partial progress

Define which bundle parts are committed together and which are gathered from external or mutable sources before save. A failed SaveChanges should not leave a misleading completed bundle under the desired relational contract. A lost response after commit should recover the same run through a command identity rather than create an indistinguishable duplicate.

If retrieval later becomes long-running, an explicit run state machine can record queued, running, completed, failed or canceled phases. Each phase needs a meaning and transition predicate. A failure state should preserve useful partial evidence without presenting an incomplete result as a full investigation. Cancellation should distinguish stopped computation from uncertain persistence.

## Historical migration

Old runs may lack document versions or algorithm identifiers. Mark those fields unknown or legacy-derived rather than fabricating exact values from current source. A migration can preserve existing JSON and attach an evidence-quality classification. Operators should know whether a run is fully reconstructable, partially explainable or only minimally associated.

Write three legacy examples: a run with sample IDs and mutable document IDs, a run with malformed output JSON and a run whose source document was deleted. Propose a display and repair path for each. Do not silently delete the run because reconstruction is incomplete; its existence may still matter for audit or user support.

## Access and revocation

The bundle can contain sensitive logs and private runbook text. Apply authorization to evidence views, not just the initial investigation command. A user who loses access should not retrieve a stored snapshot through an unprotected run endpoint. At the same time, authorized auditors may need historical evidence after a document is revoked. Define those roles and retention rules explicitly.

## Required tests

Test document edits after run creation, late log ingestion, two concurrent runs, lost response, stale approval, deleted sources, unknown payload version and unauthorized evidence access. For each, state whether the expected result is successful reconstruction, an explicit limitation or rejection. A good design does not pretend every case can be made certain.

Submit the bundle schema, state diagram, legacy policy and test matrix. The capstone passes when a reviewer can trace a displayed recommendation to the exact evidence it claims to represent and can see where that trace ends.

[Index](README.md) · [Review notes](answers/13-capstone-and-maintenance.md)
