# 10. Serialize data rather than assembling a JSON-shaped string

The investigation path uses JsonSerializer.Serialize for input and output objects. Some outbox creation paths use interpolated raw strings that look like JSON. The difference matters when a string field contains quotes, backslashes or control characters. This chapter compares those boundaries using synthetic values and proposes a focused contract test.

## Inspect an incident payload

CreateIncidentAsync includes incident ID, severity and PrimaryService in an interpolated outbox payload. The ID and enum-derived severity have constrained representations, but PrimaryService comes from trimmed request text. If that text contains a quote or backslash, direct interpolation may produce invalid JSON or a value different from the intended string.

Use a synthetic service name such as queue followed by a quote and a second word. Construct the same payload shape in an isolated harness and attempt to parse it with JsonDocument. Then serialize an anonymous object containing the same values and parse that output. The test should compare the recovered service string with the original, not merely check that the result contains braces.

## Escaping is not sanitizing

Correct JSON serialization preserves the data as a string. It does not decide whether that service name is valid for the product. Validation may reject certain names for other reasons, but serialization should still be correct for every accepted value. Treat these as separate responsibilities: domain validation defines allowed values, and the serializer encodes them faithfully.

Avoid fixing one quote example with a manual Replace call. JSON escaping includes more cases, and ad hoc escaping can double-escape or corrupt already escaped input. A serializer provides the encoding contract. The learning exercise should demonstrate why representation logic belongs at a well-defined boundary rather than scattered interpolation sites.

## Null semantics

An assignment outbox payload interpolates a nullable assigned-user value inside quotes. Consider the difference between JSON null, an empty string and a missing property. They can mean unassigned, unknown, unchanged or invalid depending on the consumer contract. A string interpolation of a nullable value may collapse distinctions that an object serializer can preserve explicitly.

Define the intended payload for unassignment. Then test a user ID, null and omission if the command permits it. The consumer should not guess that every empty string means null. A versioned payload schema can document the distinction and allow future readers to handle older messages deliberately.

## Round-trip tests

Build a table of accepted synthetic strings: ordinary ASCII, quotes, backslashes, newline, non-ASCII characters and a long value within the domain limit. For each, serialize, parse and compare the recovered value. Add a malformed message fixture to the consumer design and decide whether it is rejected, quarantined or retried. Retrying permanently malformed JSON forever is unlikely to repair it.

The current worker does not parse and deliver the payload, so a malformed message may be marked Processed under its bookkeeping behavior. A future dispatcher should validate payload version and shape before performing effects. That is a proposed extension and should have an explicit failure status rather than silently discarding the message.

## Audit evidence

When logging a malformed payload, avoid copying the entire message if it can contain private ticket or incident data. Record message ID, type, payload version, parse category and a bounded diagnostic. An authorized inspection tool can show the full payload when necessary. Useful diagnostics should preserve identity without turning broad logs into a second uncontrolled data store.

## Exercise

Submit a round-trip test plan, proposed serializer-based payload construction and a consumer error policy. Include a changed-payload version example and a null-assignment case. A strong answer distinguishes valid representation from valid business input and does not claim that JSON parsing alone authorizes an operational action.

[Index](README.md) · [Review notes](answers/05-approval-outbox-and-json.md)
