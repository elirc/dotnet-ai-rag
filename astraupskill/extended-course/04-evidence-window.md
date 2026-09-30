# 4. A bounded log sample is not the full incident

[RunIncidentInvestigationAsync](../../src/Infrastructure/Operations/OperationsService.cs) constructs a log window from two hours before incident detection through the investigation's start time. It filters logs by the incident's primary service and inclusive timestamp bounds. It counts all matching rows and separately selects at most twenty identifiers ordered by descending timestamp and then identifier.

## Count and sample answer different questions

If twenty-five logs match, evidenceCount is twenty-five while the recorded sample contains at most twenty identifiers. A screen showing the sample should not imply that only twenty matching events existed. Conversely, a count of twenty-five does not mean the operator has inspected every event. The distinction is especially important when a rare early event explains an incident but newer repetitive logs dominate the sample.

Create twenty-five matching logs with distinct times, one just before the lower bound, one exactly at the lower bound, one exactly at the upper bound and one just after it. Be explicit about whether the twenty-five already include the boundary cases. Predict the total and sample identifiers. Ambiguous fixture descriptions create incorrect expected counts even when the application is correct.

Add two logs with the same timestamp and different identifiers. The secondary ordering makes sample selection deterministic among ties. As with retrieval ties, compare identifiers rather than only the sample size. A sample containing the wrong boundary event can still have length twenty.

## Time capture and reproducibility

The current method uses DateTimeOffset.UtcNow at several points. That makes a direct test of the exact upper bound harder unless the test controls or accommodates the clock. A proposed clock abstraction would let tests supply one fixed investigation start and separate measured phase times. The abstraction is a design exercise; do not pretend it is already injected into this method.

For a test of the existing method, choose synthetic log times safely inside and outside a known interval, and record the captured bounds from persisted input. For exact boundary semantics, extract or test the query predicate with a controlled boundary in an isolated harness. Distinguish those layers so a test does not depend on a millisecond race with the real clock.

## Two reads can observe different populations

The method performs CountAsync and then a separate query for sample identifiers. Under concurrent ingestion, those two reads may observe different populations depending on database isolation and transaction scope. The inspected code does not itself wrap those reads in an explicit shared snapshot transaction. A count/sample relationship observed in an EF InMemory test does not establish a relational snapshot guarantee.

Design a concurrency exercise in which a new matching log arrives between count and sample. What should the product promise? Options include documenting approximate correspondence, recording a snapshot boundary with ingestion sequence, or using a database transaction with an appropriate isolation contract. Explain the cost and evidence needed for the chosen design. Do not add a transaction merely because it sounds stronger without defining the desired observation.

## Provenance worksheet

For each sample ID, record source service, event time, ingestion time if available, selection reason and the investigation run that referenced it. Event time and ingestion time differ when logs arrive late. A future replay of the same event-time window can contain more rows than the original investigation. To reproduce the original evidence, the stored sample IDs are more useful than rerunning a broad query alone.

Submit a boundary table, a late-arrival scenario and a count/sample consistency policy. A strong answer uses the persisted window and identifiers as evidence, states the twenty-item bound and identifies what was omitted. It never turns “no matching logs in this bounded query” into “the service had no relevant events anywhere.”

[Index](README.md) · [Answers](answers/03-evidence-and-approval.md)
