# 26. Propose one retrieval improvement with a measurable tradeoff

Choose a narrow improvement to the current lexical retriever: a synonym map, structured source filtering, a changed threshold, pre-tokenization or a different tie policy. The capstone is a reviewable proposal and evaluation package. It should not bundle every possible search feature into one rewrite or imply that a more complex system is automatically better.

## Start with an observed error

Use one failure from the labeled dataset. A timeout query may miss a deadline runbook because the exact tokens differ. A relevant body-only mention may score below threshold. An obsolete title-heavy document may outrank a current guide. Each problem suggests a different intervention. Increasing title weight does not repair a missing synonym, and lowering threshold does not establish source trust.

Write the trigger, current result, desired result and user cost. Include the query, corpus version and labels. A reviewer should be able to reproduce the failure without knowing your preferred solution. If the label itself is disputed, resolve or document that disagreement before optimizing against it.

## Define the changed contract

For synonym expansion, state whether expansion is directional, whether original terms receive different weight and how duplicates are handled. For filtering, state which source metadata is authoritative and how inaccessible or revoked documents are excluded. For threshold changes, state whether one global threshold remains appropriate across queries of different lengths. For tie changes, state the new total ordering and why it improves the user task.

Keep output explanation consistent with the new mechanism. If a document matches an expanded synonym, the result card should distinguish original query terms from expansion terms. If a filter removes obsolete documents, the evaluation should report that as source policy rather than attributing the improvement solely to scoring. Explainability requires a faithful account of the pipeline.

## Build an experiment

Use a tuning set to choose parameters and a held-out set for final comparison. Preserve the baseline results. Record per-query precision, recall or reciprocal rank under explicit conventions, plus no-relevant abstention cases. Inspect regressions even when the average improves. An operationally important miss can matter more than several easy wins.

Include a negative control where the change should have no effect. A synonym map for timeout should not reorder a pure cache query unless the design intentionally shares terms. A source filter should not alter the score of retained documents if it only changes eligibility. Negative controls help detect unintended coupling.

## Implementation plan

Identify the smallest source boundary to change and the tests that must remain stable. A pure scorer change belongs with exact arithmetic and permutation tests. A caller-side filter needs authorization and corpus-selection tests. An index needs update and stale-version tests. A UI explanation change needs contract and rendered-state checks. Do not use one test layer to claim all of them.

If data migration is required, describe ambiguous historical values and rollback limits. A tag normalization change can alter ranking without touching weights. A source-status field may need an explicit unknown state for old documents. Filling unknown with approved would fabricate authority and bias evaluation.

## Decision record

Write an accept, revise or reject recommendation based on results. Include cost, complexity, observed benefit and remaining uncertainty. Rejecting a change that does not improve the intended task is a successful experiment. The purpose is evidence-based improvement, not validating the first idea.

Submit the error case, changed contract, dataset split, result table, implementation plan and decision. Full credit requires at least one regression or limitation discussed concretely. The best proposals are small enough to review and precise enough to evaluate again after implementation.

[Index](README.md) · [Review notes](answers/13-capstone-and-maintenance.md)
