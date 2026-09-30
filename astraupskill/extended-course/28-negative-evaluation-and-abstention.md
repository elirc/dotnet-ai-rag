# 28. Evaluate when the retriever should say nothing useful matched

Search quality is often discussed through successful matches, but an incident tool must also handle irrelevant, ambiguous and underspecified queries. The current retriever can return no matches because terms are removed, no token overlaps or all scores remain below threshold. Those mechanisms are different and should be represented in the evaluation set.

## Build negative classes

Create queries that are stop-word-only, unrelated to the corpus, related through an absent synonym, too generic, or misleadingly share a popular service name. Label whether no result is appropriate and explain why. A synonym miss is a false negative if a relevant document exists; an unrelated query returning none is a correct abstention. Both can produce an empty list but have different improvement implications.

Add a corpus document that repeats a common service name in title and tags while discussing an unrelated procedure. The score can be high despite weak relevance. This tests whether metadata popularity overwhelms useful discrimination. A threshold increase may remove some false positives while also losing body-only relevant documents. The evaluation should expose that tradeoff.

## No-result communication

The current no-match recommendation directs manual investigation and avoids inventing a mitigation. A UI can explain why the retrieval returned none without overpromising: no eligible document met the current matching threshold for the captured query. It should not claim that no runbook exists anywhere or that the incident has no known cause.

Offer useful next observations, such as checking terminology, source coverage or the bounded log evidence. Avoid automatically broadening the query until something appears unless the expansion policy is evaluated and clearly labeled. Returning an irrelevant document merely to avoid an empty state can increase operational risk.

## Ambiguity is not absence

Several equally plausible runbooks may match. The retriever orders them deterministically, but that ordering does not resolve operational ambiguity. A reviewer should see multiple candidates and their evidence. A future reranker might help, yet its confidence and authority still need evaluation. The top result is a candidate, not a command.

Design a case with cache saturation and database connection exhaustion sharing terms such as timeout and service. Human labels may identify both as possible diagnostic paths until logs distinguish them. A binary relevance label can be too coarse; graded or task-specific labels may better represent useful uncertainty.

## Metrics for abstention

Track no-relevant queries separately from queries with labeled relevant documents. Measure how often the system returns any misleading candidate in the no-relevant group, and how often it returns empty despite relevant evidence. State denominators explicitly. Do not assign arbitrary perfect recall to a query with zero relevant items and then average it into a flattering score.

For small educational datasets, show the actual query table rather than only percentages. A result such as two false positives out of five negative cases is easier to audit when the five cases and labels are visible. Statistical claims about broad deployment need a larger representative process than this workshop supplies.

## Exercise

Submit at least eight negative or ambiguous queries, labels with reasons, current results and one proposed improvement. Include a no-result screen and an ambiguous-result screen. Explain how each preserves the difference between retrieval failure, corpus absence and unresolved incident diagnosis. A strong answer values appropriate abstention as a useful outcome rather than treating every empty list as a defect.

[Index](README.md) · [Review notes](answers/13-capstone-and-maintenance.md)
