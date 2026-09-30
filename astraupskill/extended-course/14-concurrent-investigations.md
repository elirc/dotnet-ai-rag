# 14. Two investigations can produce two valid runs and one latest candidate

The investigation method creates a new AgentRun and HumanApproval for each successful invocation, then updates candidate fields on the incident. It does not show a request idempotency key or an exclusive investigation claim in the inspected method. The Incident.Version property is configured as a concurrency token, but this particular path does not increment it in the same way that AddIncidentTimelineAsync does. These facts motivate a careful concurrency characterization.

## Separate records from the current summary

Two callers can investigate the same incident. Each can create its own run with its own bounded evidence window and retrieval results. Both runs may be useful historical records. The incident's AiRootCauseCandidate is a mutable summary field, so a later save can determine which candidate is currently displayed. A list of runs and the incident summary therefore answer different questions.

Decide whether the product wants to permit concurrent runs, deduplicate identical requests or allow only one in flight. None of those policies follows automatically from the word agent. If concurrent runs are allowed, the UI should identify which run produced the displayed candidate and which approval belongs to it. A copied recommendation JSON is helpful but a direct association can make the relationship easier to query.

## Concurrency token mechanics

An EF concurrency token compares the original value tracked by a context with the stored value during an update under the configured provider. If a writer changes that token, another stale context can detect a conflict. If a path updates other fields without advancing a manually managed token, two writers may share the same token value and not create the intended conflict between themselves.

Design a two-context relational test for two investigations and another for investigation versus timeline update. Record original and final Version values, run counts, approvals and candidate text. Do not infer the result solely from IsConcurrencyToken metadata. The test should reveal how the actual save operations interact with token advancement.

## Request retries

A browser can retry after losing the response to a completed investigation. Without a stable command identity, the retry can create another run and approval. This may be acceptable if every click intentionally starts a new investigation, but it can confuse operators who believe they are merely recovering the first result. Define the command's identity and user experience explicitly.

A proposed request receipt could return the original run for an equivalent retry. A proposed explicit “run again” action could generate a new identity. Changed incident content raises another question: should the same command key conflict if the input snapshot differs? Store enough input identity to make that decision meaningful.

## Candidate selection policy

If several runs exist, “latest save wins” is not necessarily the best current candidate policy. A slower run started earlier can finish after a newer run and overwrite its candidate. A proposed update predicate can compare run start sequence, incident version or an explicit selected-run field. Each choice needs a rule for what happens when the underlying incident changes during retrieval.

Build a schedule with run A starting first and finishing last, while run B starts later and finishes first. Give them different synthetic retrieval matches. Predict the current implementation's candidate under controlled saves, then define the desired policy. A deterministic test should assert the selected run identity, not only the candidate string.

## Exercise

Submit a concurrency policy, two-context test plan, lost-response contract and selected-run design. Clearly mark proposals. A strong answer allows multiple historical runs without confusing them with one mutable incident summary, and it explains exactly how version checks would enforce the chosen selection rule.

[Index](README.md) · [Review notes](answers/08-concurrency-and-provenance.md)
