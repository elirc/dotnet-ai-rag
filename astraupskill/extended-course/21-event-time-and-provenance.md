# 21. Distinguish observed time from a convenient default

CreateIncidentAsync sets DetectedAt to the captured current time and StartedAt to fifteen minutes before it in the inspected implementation. Timeline creation uses current time for occurrence and creation. Investigation then derives its log lower bound from DetectedAt. These choices create a coherent demonstration workflow, but they do not prove the incident actually began fifteen minutes earlier.

## Name the time's meaning

StartedAt can mean estimated onset, confirmed first symptom or a system default. DetectedAt can mean when monitoring detected the incident or when a user opened the record. CreatedAt is record creation. Those meanings may coincide in a demo but diverge in real operations. A data model should preserve the distinction if users rely on incident duration or evidence windows.

Use a synthetic incident that began at 08:00, was detected at 08:40 and was entered into the console at 09:10. Under the current creation method, the stored default times would be based on 09:10 rather than those supplied historical events. Explain how a two-hours-before-DetectedAt log window can still include useful data while its provenance needs accurate labeling.

## Proposed uncertainty representation

A richer incident form could accept an estimated onset with a confidence or precision label, an observed detection time and the immutable record creation time. Do not turn that precision label into a statistical confidence without a defined model. It can simply state exact, approximate or unknown. Preserve who supplied the estimate and when it was revised.

When onset is unknown, storing null or an explicit estimate is more honest than silently fabricating a precise timestamp. A default can still help a demo or initial workflow if the UI labels it as an estimate. The learning exercise is to make the provenance visible rather than treating every populated field as measured fact.

## Timeline ordering

ToIncidentDetail orders timeline events by OccurredAt. If two events share the same timestamp, a unique tie-break can make ordering deterministic. CreatedAt can differ from OccurredAt for late-entered events under a future design. An operator may need both chronological incident order and audit order of data entry.

Create two late-entered events and one correction. Design a timeline view that shows event time and entry time without implying the correction happened during the original incident. A historical audit should retain the correction relationship rather than rewriting the past silently. This matters when investigating why an earlier recommendation was reasonable given the evidence available then.

## Window reproducibility

An investigation captures logWindowEnd at its start and stores the bounds in input JSON. That supports reproducing the query definition. It does not freeze logs that arrive later with timestamps inside the window. To reproduce selected evidence, retain IDs or a snapshot as discussed earlier. Time bounds and data-version bounds solve different problems.

If clocks differ across log sources, an event-time window can omit relevant records or include misleading order. A proposed ingestion pipeline could record source timestamp and ingestion timestamp separately. Evaluation should include skewed and late events. Do not assume the application currently implements clock correction from the presence of a DateTimeOffset field.

## Exercise

Write a time-provenance table for the 08:00/08:40/09:10 scenario, then design a revised incident input contract. Include unknown onset, late event and corrected event cases. Explain how each affects retrieval and human review. A strong answer treats timestamps as claims with sources, not decorative metadata.

[Index](README.md) · [Review notes](answers/11-time-tags-and-metadata.md)
