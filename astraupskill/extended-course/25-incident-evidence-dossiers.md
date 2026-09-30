# 25. Investigate evidence that looks more certain than it is

The following synthetic dossiers test whether you can separate retrieval, persistence and operational truth. For each, write known facts, unsupported inferences, next observations and a safe recommendation. Do not execute any mitigation; the task is evidence review.

## Dossier A: the highest score is obsolete

An investigation returns Queue Archive with score eight ahead of Queue Recovery with score five. The operator sees a large score and assumes the archive is the best current procedure. The corpus metadata says the archive concerns an old deployment, but the pure scoring function does not use that fact.

Explain the ranking arithmetic and the trust gap. Propose a source-status filter or visible warning under a defined policy. Then test a current document with weak metadata so the filter does not hide the separate recall problem. A good repair does not simply boost the desired document until this one example changes order; it evaluates relevance and trust across a labeled set.

## Dossier B: twenty IDs and thirty matching logs

The run output reports evidenceCount thirty while SearchLogs output stores twenty sample IDs. A reviewer calls this corrupted evidence. Explain the intended bound and how count differs from sample. Then add a concurrent ingestion event between the two queries and discuss the weaker consistency possibility. The same numbers can be expected truncation without proving perfect snapshot correspondence.

Design a display that shows the total, sample size, selection order and captured window. Include a link or query contract for broader inspection if authorized. Do not imply the sample is statistically representative; it is selected by recency and ID ordering, not random sampling.

## Dossier C: an approval with no executable action

A Pending approval exists, but the inspected API only lists approvals and the worker only marks outbox messages processed. A stakeholder assumes the system can approve and automatically mitigate the incident. Write a capability explanation grounded in the current code. Then outline the missing decision, execution and recovery contracts as separate proposed work.

The explanation should be clear without dismissing the useful existing evidence workflow. The system already records a deterministic investigation and recommendation. The missing pieces concern authority and action lifecycle. Labeling them accurately helps plan implementation and prevents users from relying on a capability that is not present.

## Dossier D: two runs disagree

Run A starts first, reads one corpus version and finishes last. Run B starts later, reads an updated document and finishes first. Both persist valid run records, and the incident's candidate ends with A's older recommendation under a controlled last-save schedule. The operator sees only the incident summary.

Propose a selected-run policy and provenance display. Decide whether latest-started, latest-completed, explicitly approved or manually selected should determine the current candidate. Each policy has different implications. Store the selected run identity so the UI can explain the choice, and prevent a stale completion from overwriting it contrary to policy.

## Dossier E: a lost response creates another approval

The first investigation commits but its response is lost. The browser retries and creates a second run and Pending approval. The operator sees two nearly identical entries and cannot tell whether they represent intentional reruns. Design a command receipt or explicit run-again identity that separates retry recovery from a new investigation.

Include changed incident input under the same key. Returning the first result for materially different input without a conflict can mislead the user. The receipt should retain the input snapshot identity and define equivalence. It should also enforce current access when returning the original result.

## Review task

Choose one dossier and write a reproduction with synthetic fixtures and a decisive assertion. Add an unsafe shortcut that the test should prevent, such as executing a top-ranked runbook automatically, treating a truncated list as complete or retrying an uncertain command with a new identity. Then write a plain-language user explanation and a technical operator handoff that agree on the facts.

The assessment rewards calibrated conclusions. A high score, a Completed status, a Pending approval and a Processed outbox row each have precise local meanings. None should be expanded into a claim about incident cause or external action without additional evidence.

[Index](README.md) · [Review notes](answers/12-integration-and-incidents.md)
