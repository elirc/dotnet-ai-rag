# 7. Run a transparent ranking laboratory

The [retrieval laboratory](labs/retrieval_lab.py) is a Python teaching model for a deliberately limited ASCII fixture. It mirrors the documented field weights, query stop words, threshold and bounded ordering for that fixture. It does not import the C# application or reproduce every .NET string and Guid comparison edge case. Use it to inspect arithmetic, then use production tests for the actual runtime contract.

## First run

Run `python labs/retrieval_lab.py` after predicting the queue example from chapter one. The program asserts expected scores, repetition invariance, threshold behavior, an empty stop-word query, a result-count bound and input permutation stability for the supplied identifiers. It prints a small result table so you can connect each assertion to a visible document.

The fixture uses synthetic content and numeric identifiers. Numeric ordering is sufficient for this teaching example but is not a general replacement for Guid comparison. The model intentionally states that limit. If you port it into a production test, use the application's actual identifier type and comparer, and keep the expected sequence independently specified.

## Mutation exercises

Change the title weight from four to one in a disposable copy. Predict which fixture scores change before running it. The reference assertions should detect the difference. Then remove query-term deduplication and search for repeated queue terms. Explain why a repeated query should not increase a document's score under the current contract.

Next remove the identifier tie-break and reverse the fixture input. Construct two equal-score, equal-title documents so the result reveals the missing key. Finally change the threshold comparison from greater-than-or-equal to strictly greater-than. The exact-boundary document should disappear. These mutations target meaningful contract properties rather than superficial formatting.

## Extend with a labeled dataset

Add six synthetic incident queries and at least eight documents. Write relevance labels before tuning weights. Include one no-relevant case and one lexical mismatch where the useful document uses a synonym. Calculate actual-list precision and labeled recall using the conventions from chapter three. Keep no-relevant queries in a separate abstention table.

Compare one threshold change. Report both recovered relevant matches and newly admitted irrelevant matches. A change that improves recall while reducing precision may still be useful, but the decision depends on the user's investigation cost. State that tradeoff instead of selecting whichever metric happens to improve.

## Independent capstone: evidence-aware retrieval review

Design a reviewer screen for a completed deterministic investigation. It should show the captured incident context, log window, evidence count, bounded sample, matched runbooks and pending approval. Explain heuristic scores in plain language and preserve the distinction between candidate relevance and verified cause. Include a no-match screen with a useful next step.

Add a provenance contract that can reproduce the runbook content reviewed at the time. Propose how a later document edit and a later incident update affect an outstanding approval. Include a concurrency test for two decisions on the same approval version. Do not implement mitigation execution as an incidental part of the learning exercise.

The capstone must include a claim/evidence matrix. For each claim, identify whether it rests on source inspection, a pure model check, a service test, a relational test or human relevance labels. At least one claim should remain explicitly unproved with a proposed next test. This prevents a polished UI from making the evidence look stronger than it is.

## Assessment

Score arithmetic correctness, deterministic ordering, evaluation design, provenance, authorization and communication separately. A high overall score cannot compensate for presenting a heuristic as model confidence or for treating a pending approval as permission to execute. Require a worked no-match case and an explanation of count versus sample. Those cases reveal whether the learner understands the limits of the investigation.

[Index](README.md) · [Answers](answers/04-lab-and-evidence.md)
