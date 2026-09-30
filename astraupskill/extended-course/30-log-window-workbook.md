# 30. Calculate a complete bounded-evidence fixture

This workbook provides an independent log-selection oracle. Fix incident detection at 12:00 UTC and investigation start at 12:30 UTC on a synthetic day. The current lower bound is therefore 10:00 UTC and the upper bound 12:30 UTC, both inclusive. The primary service is queue. Use the following groups without adding unstated records.

Group A contains eighteen queue logs, one per minute from 12:01 through 12:18. Group B contains three queue logs at exactly 12:20 with IDs ending 101, 102 and 103. Group C contains one queue log exactly at 10:00. Group D contains one queue log at 09:59:59 and one at 12:30:01. Group E contains two cache-service logs at 12:10. Group F contains one queue log exactly at 12:30.

## Count eligibility first

List eligible groups before sorting. Time alone does not decide eligibility because the service predicate also matters. The cache logs are within the window but belong to another service. The two Group D logs match service but fall outside opposite boundaries. The exact-boundary logs in Groups C and F are eligible under the inclusive comparisons.

Calculate evidenceCount from eligible rows. Then order them by descending timestamp and ascending identifier for timestamp ties. The sample takes the first twenty. Determine which of the oldest eligible queue logs are omitted from the sample and explain why omission does not mean they failed the query predicate.

## Tie reasoning

The three 12:20 rows come immediately after the 12:30 row in the sample. Their IDs decide order within that timestamp. Use the selected numeric Guid suffixes and verify actual ordering in a production-type test if implementing the fixture. Reversing insertion order should not change the intended ordered IDs under the explicit tie rule.

Now change only the ID order of the input enumeration. The query ordering should still determine output. Then remove the secondary ordering in a disposable query model. Explain why repeated runs that happen to return the same order do not prove a deterministic contract. The decisive test supplies tied timestamps and checks the explicit key sequence.

## Late arrival

After the original investigation saves, insert a queue log whose event time is 11:00 but whose ingestion occurs at 13:00. A later replay of the same time predicate now counts one more row. The original stored sample IDs still describe what the earlier selection recorded. This demonstrates why a time window is a query definition rather than an immutable snapshot of the database population.

Propose an ingestion-sequence cutoff if exact population replay is required. The cutoff needs to be captured consistently and stored with the run. It should not replace event time; the two dimensions answer different questions. A log can arrive late while still being operationally relevant to the incident.

## Concurrent count and sample

Suppose the late log is inserted between CountAsync and the sample query instead. The count may describe the earlier population while the sample query sees the new one, depending on the database observation contract. In this fixture, the late log is old enough that it may not enter the top twenty, but it still illustrates a population difference.

Change its event time to 12:25 and repeat the reasoning. It can now enter the sample and displace another row while the recorded count remains from the earlier read. Design a controlled relational test if the product needs to characterize this schedule. Do not claim EF InMemory establishes the configured relational isolation behavior.

## Operator explanation

Write a concise explanation for a reviewer who notices that rerunning the window produces a different count. Include late ingestion, selection bounds and sample retention as possible causes. The explanation should not immediately label the original evidence corrupted. It should identify what additional ingestion metadata or snapshot evidence would distinguish expected evolution from an actual bug.

Submit the eligible count, exact sample ordering by group, omitted rows and both late-arrival variants. A strong answer separates eligibility, ordering, truncation and population version. Those four concepts are easy to conflate when looking only at one total and a list of IDs.

[Index](README.md) · [Answers](answers/14-final-calculations.md)
