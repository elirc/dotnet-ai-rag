# 1. Explain a retrieval result without invoking a model

Read [LexicalRetriever](../../src/Application/Retrieval/LexicalRetriever.cs). The current retriever tokenizes text, removes selected query stop words, scores exact token membership in document fields, applies a threshold and returns a bounded ordered list. It does not call a language model or produce a calibrated probability. Understanding those mechanics lets you explain both useful matches and surprising misses.

## Calculate one term

For a query term, membership in a document title contributes four points, membership in tags contributes three, and membership in content contributes one. The same term can contribute in all three fields for a total of eight. Repeating the term many times within one field does not increase that field's contribution because the document tokens are converted to sets.

Use query queue. Document A has Queue Recovery as its title, operations as tags and “inspect queue depth” as content. It scores five. Document B has Worker Guide as title, queue as tags and unrelated content, so it scores three. Document C has a generic title and tags but mentions queue in content, so it scores one. With the default minimum score three, A and B survive and C does not.

This example shows why absence from the result list is not proof that a document contains no matching word. C matches, but its score is below threshold. It also shows why title and tag quality influence retrieval. Adding a relevant tag can make a document cross the threshold without changing its body. That may improve retrieval, but indiscriminately adding popular tags can degrade precision.

## Multiple terms and repetition

The query terms are deduplicated after stop-word removal. Query “queue queue” behaves like queue for scoring. Query “queue backlog” can contribute from both terms. A document with queue in the title and backlog in content scores five even if neither term occurs elsewhere. A document with queue only in content and backlog only in content scores two and remains below the default threshold.

Write the contributions per term before summing. This makes it easier to detect a mistaken assumption that the retriever counts occurrences or requires every query term. It does neither in the inspected code. A high score can arise from several field memberships without indicating a confirmed incident cause.

## Tokenization is part of behavior

Tokens are lowercase matches of the expression `[a-z0-9]+`. Punctuation splits terms. “queue-depth” yields queue and depth. “HTTP500” becomes one token, http500, rather than http and 500. The pattern is ASCII-oriented: non-ASCII letters do not receive the same treatment as ordinary English letters. Treat multilingual behavior as a concrete design question, not an assumed capability.

The stop-word set includes generic words such as the and for, and domain words such as incident, investigate and service. Those terms are removed from the query, not from every document field before all other processing. A stop-word-only query returns no matches. Explain why a query containing “service queue” still searches for queue, while “investigate service incident” becomes empty.

## Worksheet

Create four documents and calculate results for queue, queue backlog, queue-depth, HTTP500 and a stop-word-only query. For each, list query tokens after filtering, per-field contributions, total score, threshold decision and output position. Include a document that matches below threshold and a document whose repeated body term does not change its score.

Then propose a synonym feature for timeout and deadline. Identify what would change: query expansion, document normalization, scoring or a separate retrieval stage. Explain the new false positives it could introduce. Label the proposal clearly; the current exact-token implementation does not perform semantic similarity merely because the product concerns AI operations.

## Review standard

A correct explanation can reproduce the ranking with arithmetic and source references. It should not use phrases such as “the AI is confident” to describe a field-weight sum. A strong answer also explains one failure mode of the current tokenizer and one tradeoff of the proposed synonym design. The point is to make retrieval behavior inspectable before adding more sophisticated machinery.

[Index](README.md) · [Answers](answers/01-ranking.md)
