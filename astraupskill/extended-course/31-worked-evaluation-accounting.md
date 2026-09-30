# 31. Calculate aggregate metrics without hiding difficult queries

Use three synthetic queries with fixed human labels. Query Q1 has two relevant documents and returns three items, two relevant, with the first relevant item at rank one. Q2 has four relevant documents and returns one item, which is relevant at rank one. Q3 has one relevant document and returns three irrelevant items. A fourth query Q4 has no relevant documents and returns two irrelevant items. Keep Q4 separate for abstention analysis.

## Per-query results

Under the course's actual-returned-list precision convention, Q1 precision is two thirds and recall is one because both relevant documents were found. Q2 precision is one and recall is one quarter. Q3 precision and recall are zero, and reciprocal rank is zero because no relevant item appears. Q1 and Q2 each have reciprocal rank one.

Calculate macro precision and macro recall over Q1 through Q3 by averaging the per-query values. Then calculate micro precision by summing relevant returned items and total returned items. These aggregates differ because macro gives each query equal weight while micro gives more weight to queries returning more items. Neither should be presented without its definition.

## Work the arithmetic

The three queries return seven items, three relevant. Micro precision is therefore three sevenths. The total labeled relevant count is seven, with three retrieved, giving a pooled recall of three sevenths under this fixture. Macro recall is the average of one, one quarter and zero. Macro precision is the average of two thirds, one and zero. Write exact fractions before decimal approximations to avoid rounding confusion.

The mean reciprocal rank over these three is two thirds. That favorable-looking number reflects two queries finding a relevant item first, while Q3 fails entirely. A report showing only reciprocal rank could hide poor coverage of Q2's additional relevant documents and the complete miss on Q3. Use per-query results to preserve those details.

## Handle Q4 honestly

Q4 has no recall denominator because the label set contains no relevant documents. Report it in the no-relevant group and note that the system returned misleading candidates. Do not assign it perfect recall and average that into the positive-query score. The abstention failure is a separate important result.

If Q4 returned nothing, that would be a correct abstention under the label policy, but it would still not prove the system knows no useful document exists outside the evaluated corpus. The claim is scoped to the corpus and labels. Evaluation is always about a defined task and dataset, not universal knowledge.

## Compare a proposed change

Suppose a lower threshold adds two items to Q2, one relevant and one irrelevant, while Q1 and Q3 stay unchanged. Recalculate Q2 precision and recall, micro precision and pooled recall. The change improves Q2 recall but reduces its precision. Decide whether the tradeoff is acceptable for an operator who can inspect at most three candidates.

Now suppose the change also returns five irrelevant items for Q4. A positive-query recall improvement can coexist with worse abstention behavior. The final decision should consider both. This is why tuning only on queries known to have relevant documents can produce a system that always suggests something even when it should abstain.

## Label uncertainty

If reviewers disagree about one Q1 document, preserve the disagreement and recalculate a sensitivity range or use graded relevance under a defined policy. Do not choose whichever label makes the preferred algorithm look better. A small dataset is especially sensitive to one label, so show the actual judgments and reasons.

Submit exact fractions, rounded values with stated precision, the before/after table and a decision paragraph. A strong answer uses metrics to expose tradeoffs rather than compress them into one flattering score.

[Index](README.md) · [Answers](answers/14-final-calculations.md)
