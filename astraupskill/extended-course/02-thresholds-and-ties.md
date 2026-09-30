# 2. Bounded results must still be deterministic

After scoring, the retriever keeps matches at or above minimumScore, orders by descending score, then by title using an ordinal case-insensitive comparison, then by document identifier. It takes topK results after that ordering. A deterministic final tie-break matters whenever document enumeration order can vary between database reads or test fixtures.

## Stable sort is not enough

Suppose A and B have equal scores and equal titles. If a sort preserves input order for equal keys, reversing the input list reverses the output. That is stable sorting, but it is not input-order-independent ranking. The final document identifier comparison supplies an additional key so the result does not depend on which tied document happened to arrive first.

Construct two equal-score documents with the same title and distinct identifiers. Search in both input orders and compare returned identifier sequences. Then add a third document with a title differing only by letter case. The title comparison can treat those titles as equal, making the identifier tie-break relevant again. Use actual Guid comparison behavior in production tests rather than assuming every string representation sorts identically in every language.

## Threshold before truncation

A below-threshold document does not become eligible simply because fewer than topK documents remain. The threshold is a relevance gate and topK is a result-count bound. With topK three and only one qualifying document, the method returns one. Filling the list with weak matches would be a different product policy and would need explicit evidence that the extra suggestions help users.

Nonpositive topK or minimumScore returns an empty list in the current implementation. This is a specific input policy. It differs from clamping topK to one or treating a zero threshold as “include everything.” Test the actual behavior so a future change does not silently alter callers' assumptions.

## Ranking stability versus ranking quality

Deterministic results are easier to test and explain. They are not necessarily more relevant. If the tie-break chooses alphabetically earlier titles among equally scored documents, a runbook's title can affect whether it appears in a small topK list. That is reproducible, but it may not match a human's preferred relevance ordering.

Create a tie where one document is operationally useful and another is obsolete, yet both have the same lexical score. The current ranking does not inspect freshness or approval status in LexicalRetriever itself. Investigate what documents the calling service supplies before claiming that obsolete material is excluded. A future filtering stage could enforce visibility or publication status, but its interaction with evaluation must be measured.

## Boundary test design

Write tests for score exactly equal to minimumScore, one below it, topK zero, topK negative, minimumScore zero and tied titles. Add a permutation test over a small document set. The assertion should compare identifiers and scores, not just result length. Result length alone can pass when the wrong document survives truncation.

For a property-style test, generate permutations of a fixed fixture and require the same output sequence. This explores enumeration order, not arbitrary text correctness. Keep the claim narrow. A second property can assert that every returned score meets the threshold and count never exceeds a positive topK. A third can assert query-term repetition leaves scores unchanged.

## Product exercise

Propose displaying why a result matched: terms, field contributions and total heuristic score. The current result includes matched terms and total score but not every field contribution, so a detailed explanation panel may require an extended result contract. Avoid displaying the score as a percentage. If you normalize it for layout, explain that the normalization still does not create a probability of correctness.

Submit a boundary-test table and a mock explanation card. A good card helps the operator decide what to inspect next. It should not imply that deterministic ordering certifies a runbook as safe to execute. Retrieval selects candidates; operational judgment still evaluates applicability.

[Index](README.md) · [Answers](answers/01-ranking.md)
