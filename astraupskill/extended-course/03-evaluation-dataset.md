# 3. Measure relevance separately from implementation correctness

A ranking test can prove the weights and tie-breaks behave as written. It cannot prove that the returned runbooks help investigate real incidents. Evaluation adds a different question: for a defined set of incident-like queries, which documents should be considered useful, and how often does the retriever surface them within its bounded list?

## Build a small labeled set

Use synthetic incidents with no private operational data. Include queue backlog, authentication failures, database connection saturation, an unrelated printer problem, a stop-word-only query and a terminology mismatch. For each query, have a reviewer label relevant document identifiers before examining the retriever's result. Record the reason for each label, not just a binary flag.

Some documents may be relevant to diagnosis but unsafe as immediate mitigation. Keep that distinction in the labels. A runbook explaining how to collect evidence can be useful even when it does not prescribe a fix. A document mentioning the same service name can be irrelevant if it concerns an unrelated subsystem. Lexical overlap is a feature of the retriever, not the definition of human relevance.

Separate a tuning set from a held-out set if you plan to change weights or thresholds. If every decision is optimized against the same handful of examples, the measured improvement may simply reflect those examples. For a tiny educational corpus, a manual held-out split is sufficient to demonstrate the principle. Do not present its score as production readiness.

## Calculate metrics with explicit denominators

Precision at a cutoff asks how many returned items within the evaluated cutoff are relevant, under a stated denominator convention. Recall asks how many labeled relevant documents were retrieved out of all labeled relevant documents for that query. Reciprocal rank focuses on the position of the first relevant item. Each metric emphasizes a different user cost.

For this worksheet, define precision over the actual returned list, and separately record empty-result queries. If two documents are returned and one is relevant, precision is one half. If there are four relevant documents in the label set and one was returned, recall is one quarter. If the first relevant document is second, reciprocal rank is one half. These conventions must be stated because other evaluation setups use fixed cutoffs and different handling of empty cases.

For a query with no relevant documents, recall has a zero denominator. Do not quietly treat that as a perfect score. Report the case separately and assess whether the retriever abstained appropriately. An unrelated query that returns confident-looking suggestions is a distinct failure mode from a relevant query that returns none.

## Inspect errors rather than only averages

A high average can hide a critical miss. Create an error taxonomy: tokenization mismatch, stop-word removal, missing synonym, weak metadata, stale document, tie truncation and insufficient document visibility. Assign each failed query to one or more causes using evidence. This directs improvements toward specific mechanisms instead of changing weights at random.

For example, a useful runbook using deadline while the query uses timeout may fail because exact tokens differ. Raising title weight will not repair that mismatch. A document that matches only in content with score one may fail because of threshold policy. Lowering the threshold could recover it while also admitting many irrelevant body mentions. The evaluation set should reveal both effects.

## Experiment report

Compare the current weights with one proposed variant on the tuning set, then evaluate the chosen variant once on held-out examples. Record document corpus version, query set, labels, metric definitions and every changed parameter. Keep the original result table so the reviewer can inspect regressions, not only a favorable aggregate.

Your report passes when it distinguishes implementation tests from relevance judgments, handles empty and no-relevant cases explicitly, and explains at least two errors with source-grounded mechanisms. It should not claim clinical-style confidence or operational causality from a small lexical benchmark. The useful output is a reproducible basis for a narrow retrieval improvement.

[Index](README.md) · [Answers](answers/02-evaluation.md)
