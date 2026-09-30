# 20. An independent ranking casebook

Use the fixed synthetic corpus below for every case. Calculate answers before opening the separate review guide. IDs are distinct Guids whose final numeric suffixes are 1 through 9. The earlier suffix sorts before the later suffix for the selected fixture values in the actual-source harness. All text is ASCII so tokenizer and title-comparison behavior can be reasoned about directly.

| ID | Title | Tags | Content |
|---|---|---|---|
| D1 | Queue Recovery | worker backlog | queue lag restart |
| D2 | Queue Metrics | queue metrics | depth lag queue |
| D3 | Worker Recovery | worker | backlog queue |
| D4 | Cache Recovery | cache memory | cache eviction |
| D5 | Auth Token | session | token expired login |
| D6 | Database Pool | database connection | pool exhausted connection |
| D7 | QUEUE RECOVERY | worker backlog | queue lag restart |
| D8 | Queue Archive | obsolete queue | queue old deployment |
| D9 | Empty Notes | general | printer toner |

## Case A: a common term with an obsolete candidate

Search queue with default bounds. Compute every nonzero score and order the qualifying documents. Pay particular attention to D8: the word obsolete is metadata in this fixture, but the retriever does not interpret it as a trust filter. Explain why a high lexical result can still require operational review. Your answer should name the three returned IDs and identify a useful document excluded by the topK bound.

## Case B: repeated query language

Search “queue queue queue.” Compare scores and ordering with case A. Then repeat queue several times inside D1's content while keeping all other fields unchanged. Explain separately why repeated query terms and repeated field occurrences do not change the current score. These are two deduplication locations, so identifying only one is an incomplete answer.

## Case C: a second term creates a larger tie

Search “queue backlog.” Calculate contributions from both terms for D1, D2, D3, D7 and D8. Several documents now share the highest score. Determine how title and ID ordering decide the bounded result. Explain why adding a relevant query term does not necessarily move the human-preferred runbook to first place under this scoring rule.

## Case D: worker-focused retrieval

Search worker. Compare the title-and-tag match in D3 with tag-only matches in D1 and D7. State which document is first and how the remaining tied documents are ordered. Then set topK to one. The result should reflect the current field weights, not your intuition about which recovery guide is broader.

## Case E: body-only operational language

Search restart. The term appears in two recovery bodies. Predict the default result and explain the threshold effect. Repeat with minimumScore one in the isolated exercise. What new matches become eligible, and why is this change a relevance-policy decision rather than merely a bug fix?

## Case F: punctuation and numeric tokens

Search “queue-depth.” List the tokens and compute the new score for D2. Compare it with D8 and the recovery documents. Then search “HTTP500” against this corpus and explain why it does not match separate imagined words http and 500. Do not add unseen document content to make the query succeed.

## Case G: all generic terms

Search “investigate the incident service.” Show the query terms after stop-word removal and the result. Then append cache and calculate the result. Explain why an empty filtered query is a valid abstention rather than an exception or a reason to return the first three documents.

## Case H: threshold equality

Search session. D5 contains it only in tags. Test minimumScore three, four and zero. Explain the exact current behavior for each. A zero threshold does not mean include everything in this implementation. This case is designed to catch a test writer who remembers a generic search convention instead of the inspected guard.

## Case I: field combination

Search “database pool connection.” Calculate D6's contribution for each term before summing. State why the total is not a percentage and does not become a probability merely because it is larger than the queue scores. Different queries can have different numbers of distinct terms and therefore different possible score ranges.

## Case J: title-case ties

Search restart with minimumScore one and topK two. D1 and D7 have the same score and titles equal under the case-insensitive comparer. Reverse the corpus input order. Predict whether the returned ID order changes. Explain the role of the final Guid key and why a stable sort without that key would answer a different question.

## Case K: a metadata edit

Add restart to D1's tags only, then search restart with default threshold. Compare D1 and D7. This is a small example of metadata affecting retrieval without a body change. Describe what a relevance evaluation should check before encouraging authors to add more tags to every document. Popular tags can improve recall for one query while admitting irrelevant results for another.

## Case L: a missing relevant synonym

Assume a human labels D6 relevant to a query about connection timeouts, but the query uses only deadline and no corpus document contains deadline. Predict the current result. Propose a synonym expansion and one false-positive counterexample. Keep the human relevance label separate from the implementation's exact-token matching ability.

## Case M: result bounds

Run queue with topK one, two, twenty and zero. State the returned count and IDs where practical. The large bound cannot create below-threshold matches, while the zero bound returns empty. Explain why result-count bounds and relevance thresholds are independent controls and should be tested independently.

## Case N: explanation quality

Design a result card for D8 in case A. It should show why it matched and that the source is obsolete under the fixture's human interpretation. The current pure result does not include a structured approval status, so distinguish fields directly returned by the retriever from metadata a richer caller would need to attach. Avoid displaying an unlabeled confidence bar.

## Case O: evaluation disagreement

Two reviewers disagree about D2 for queue backlog: one considers metrics useful evidence gathering, while the other labels only direct recovery instructions relevant. Write a label policy that resolves the disagreement or records graded relevance. Recalculate one metric under both label sets. This case has no single numeric answer until the relevance definition is stated; recognizing that is part of the assessment.

## Submission

Provide a score worksheet, ordered IDs and a short explanation for every case. Mark arithmetic facts, human labels and proposed changes separately. The casebook passes when the learner can predict exact results and explain why those results still need evaluation and authority checks before operational use.

[Index](README.md) · [Casebook answers](answers/10-ranking-casebook.md)
