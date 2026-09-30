# Review notes: capstones, abstention and maintenance

The retrieval proposal should begin with an observed labeled error and choose an intervention matching its cause. Synonym misses, threshold misses and source-trust problems require different mechanisms. Preserve baseline results, use a held-out set, inspect regressions and include a negative control. A rejected proposal can be a successful experiment when evidence shows no useful benefit.

The versioned evidence bundle should define its reconstruction goal before choosing retained data. Exact ranking, operator explanation and action authorization need overlapping but different evidence. Legacy runs should retain unknown fields as unknown, with an evidence-quality classification rather than fabricated document versions or timestamps. Access rules apply to stored snapshots as well as current documents.

Abstention evaluation separates correct no-result cases from false negatives and ambiguous candidates. A stop-word-only query is not the same as a synonym miss. No-relevant queries need explicit denominators and should not receive arbitrary perfect recall. A top result remains a candidate even when ordering is deterministic.

Maintenance follows claims across source, tests, schemas and UI. A new model path should not rewrite old deterministic metadata. A new dispatcher should update only the delivery capabilities actually implemented. Historical test reports remain dated; new execution evidence records commands and scope. The final portfolio should make current behavior, proposed work and remaining uncertainty easy to distinguish.

[Index](../README.md)
