# Answers: independent ranking casebook

For case A, D2 and D8 each score eight for queue because the term appears in title, tags and content. D1 and D7 each score five from title and content. D3 scores one from content and is excluded by the default threshold. The order begins D8, D2, D1, D7 because Queue Archive precedes Queue Metrics, then the case-insensitive recovery titles tie and IDs decide. Default topK returns D8, D2 and D1. Obsolete metadata does not alter the pure scoring rule.

Case B has the same scores. Query terms are distinct, so repeated queue is scored once. Each document field is a set of tokens, so repeated occurrences in D1's body do not increase the content contribution. These are independent mechanisms. Removing either can change repetition behavior under an appropriate fixture.

For case C, D1 and D7 gain three for backlog in tags, reaching eight. D2 and D8 remain eight. D3 has one for queue and one for backlog, totaling two and staying below threshold. The top three remain D8, D2 and D1 under title and ID ties. Adding backlog increases some scores but does not force the recovery guide to first place because the ordering still applies to tied totals.

For case D, D3 scores seven for worker from title and tags. D1 and D7 score three from tags. The order is D3, D1, D7, and topK one returns only D3. For case E, restart gives D1 and D7 one point each from content, so default threshold returns none. With threshold one, both qualify and D1 precedes D7. Lowering the threshold can recover body-only matches while admitting more weak mentions elsewhere.

For case F, queue-depth becomes queue and depth. D2 scores nine: eight for queue and one for depth in content. D8 remains eight, and D1/D7 remain five. The default top three are D2, D8 and D1. HTTP500 is one lowercase token and matches nothing in the supplied corpus. The tokenizer does not split letters from digits within the pattern.

Case G removes every original query term, producing no matches. Appending cache leaves cache, and D4 scores eight from title, tags and content. This is ordinary deterministic abstention followed by an exact match, not a model inference. Returning arbitrary documents for the empty query would be a different and potentially misleading policy.

For case H, session gives D5 three points from tags. It qualifies at threshold three, is excluded at four, and the nonpositive-threshold guard returns empty at zero. Case I gives D6 seven for database, five for pool and four for connection, totaling sixteen. The total depends on term count and field membership. It has no calibrated probability interpretation and is not comparable to every other query as if it were a confidence percentage.

Case J returns D1 then D7 regardless of input order for the chosen IDs. A stable sort without a final distinguishing key would preserve the input order of otherwise equal elements and could reverse when the input reverses. The actual Guid comparison is part of the production contract; the selected fixture IDs make the expected order explicit.

For case K, D1 gains three tag points in addition to its one content point, reaching four for restart. D7 remains one and is excluded. The metadata edit improves this query's retrieval but can affect other queries. Evaluation should test irrelevant uses of the added tag and avoid optimizing only the motivating example.

Case L returns no match for deadline in the fixed corpus. A proposed expansion to timeout or connection-related terms needs a defined mapping and evaluation. A false positive could be a business deadline document unrelated to network timeouts. The human label establishes desired relevance, not a token match the current implementation can discover automatically.

For case M, queue with topK one returns D8; two returns D8 and D2; twenty returns all four qualifying documents D8, D2, D1 and D7; zero returns none. D3 does not enter merely because extra capacity exists. Threshold and count limits remain separate.

Case N should explain title/tag/content overlap and present obsolete status as additional trusted metadata if the caller supplies it. The current RetrievalMatch provides ID, title, score and matched terms, not a full trust record. A richer result card needs a corresponding data contract. It should invite review, not execution.

Case O requires a label definition before a metric can be calculated. Diagnostic usefulness and direct mitigation usefulness are different relevance criteria. A graded label can preserve both, or the evaluation can report separate tasks. A strong answer documents disagreement and resolves it consistently across queries instead of changing labels only where a preferred algorithm performs poorly.

Review the worksheet for independent arithmetic, exact identifiers and explicit uncertainty. Deduct credit for treating score sixteen as sixteen percent, filling empty results with arbitrary documents or allowing the obsolete tag to magically remove D8 without a filtering rule. The point is to understand the implemented mechanism and then evaluate whether it serves the intended task.

[Index](../README.md)
