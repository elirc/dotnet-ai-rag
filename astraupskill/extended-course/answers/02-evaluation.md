# Answers: relevance evaluation

Under the worksheet's actual-returned-list convention, one relevant result among two returned gives precision one half. Retrieving one of four labeled relevant documents gives recall one quarter. A first relevant item in position two gives reciprocal rank one half. The report must state its conventions and separately handle empty results and queries with no relevant documents.

A no-relevant query has no recall denominator. Treating it as perfect recall hides the abstention problem. Record whether the retriever returned misleading candidates and evaluate that behavior separately. A stop-word-only query is a useful implementation case, while an unrelated natural-language query tests a different relevance failure mode.

The synonym mismatch cannot usually be repaired by increasing a weight on a term that never matches. A below-threshold body match can be recovered by lowering the threshold, but that also admits other weak body mentions. Inspect the per-query regressions before accepting a better aggregate. A held-out set helps reveal whether the chosen change merely fits the tuning examples.

Labels should include reasons and distinguish diagnostic usefulness from immediate mitigation safety. A document can be relevant to gathering evidence without authorizing an action. A high lexical score can be irrelevant when the shared terms describe another subsystem. This is why human relevance labels and implementation assertions answer different questions.

A strong experiment report preserves the corpus version, queries, labels, parameter changes and result tables. It describes the small dataset as an educational evaluation, not as evidence of production-wide reliability. The next experiment should address a specific observed error class rather than adding complexity without a measured purpose.

[Index](../README.md)
