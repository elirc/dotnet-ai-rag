# 23. Names containing AI do not establish model behavior

Ticket creation builds a string beginning with Mock triage and sets suggested priority and category from parsed request values. Incident investigation uses deterministic lexical retrieval and explicitly reports modelUsed false. Several domain fields contain AI in their names. A reader must follow the implementation rather than infer a model call from a field or class name.

## Trace the ticket suggestion

CreateTicketAsync parses or defaults priority and category, then constructs AiSummary from those values and a fixed instruction to review logs and incidents. AiSuggestedPriority and AiSuggestedCategory mirror the selected values. This is a deterministic placeholder path in the inspected source. It does not evaluate a model's classification quality or consume model tokens.

Create two tickets with identical descriptions but different explicit priorities. Predict the suggested priority fields. Then omit priority and observe the configured default at the service boundary. The exercise demonstrates that the suggestion is derived from input parsing, not independent evidence that a model agreed with the user's classification.

## Communicate mode explicitly

The investigation output's deterministic_retrieval mode, false modelUsed and null modelConfidence are useful because they make the execution mode explicit. A UI should preserve those distinctions. Displaying an AI confidence badge from heuristicScore would contradict the output contract. Displaying a mock summary as an independently generated diagnosis would also mislead users.

Propose a shared result envelope with execution mode, algorithm or model version, input snapshot, evidence references, cost accounting and uncertainty fields. Some fields may be null for deterministic runs. Null should mean unavailable or not applicable under a documented contract, not zero confidence or a failed model call by default.

## Future model integration

If a future feature calls a model, it needs its own adapter, timeout and retry behavior, prompt/input provenance, output validation and evaluation. Do not retrofit modelConfidence as a number merely because the model produces fluent text. A confidence value needs a defined interpretation and calibration evidence if users are expected to rely on it.

The model's output should remain a proposal or summary until the application validates structured fields and enforces authority. Retrieved documents and logs can contain misleading instructions or sensitive content. A model adapter should not grant those inputs authority over tool execution. The existing human-review boundary is a useful place to make that distinction explicit.

## Cost accounting

The deterministic investigation records zero estimated tokens and cost. That is consistent with no model call in this path. It does not mean database, CPU or storage work has no cost. A future cost dashboard should distinguish provider charges from local resource usage and mark estimates as estimates.

Create a proposed run ledger containing provider request identity, measured token usage when returned, estimated cost inputs and billing-version metadata. Do not calculate current provider prices from memory in this course. An implementation task should verify the active pricing contract from official sources. Here the learning objective is accurate accounting provenance, not a particular price.

## Exercise

Write user-facing labels for mock triage, deterministic retrieval and a hypothetical model-assisted proposal. Each should state what happened without overstating intelligence or certainty. Then design tests that ensure deterministic output keeps modelUsed false and confidence null. A strong answer treats names and marketing labels as weaker evidence than the executed path.

[Index](README.md) · [Review notes](answers/11-time-tags-and-metadata.md)
