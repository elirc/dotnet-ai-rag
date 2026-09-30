# 6. Match the test to the claim

Netairag has older Phase 1 material under docs/upskill, a broader docs2.0 curriculum and the newer Astra retrieval route. They serve different purposes and contain historical descriptions. This extension anchors its current behavior claims to LexicalRetriever and RunIncidentInvestigationAsync. Use older lessons for their stated exercises, but verify source before treating old feature inventories or execution reports as current.

## Four useful test layers

A pure retriever unit test can establish tokenization, weights, threshold behavior and ordering for supplied documents. It should use small exact fixtures with expected identifiers and scores. It does not establish database filtering, authorization or persistence because those concerns are outside the pure function.

A service test using EF InMemory can inspect created AgentRun, tool-call and approval objects and serialized JSON. It can verify that authorization happens before these writes in the exercised path. It does not establish PostgreSQL transaction isolation, SQL translation, indexes or relational constraints. An in-memory provider is useful evidence when its scope is stated accurately.

A relational integration test can examine actual query translation, persisted relationships, transaction behavior and database constraints in a disposable instance. It should identify the provider and schema used. It still does not establish retrieval relevance for real incidents merely because the records persisted successfully.

An evaluation dataset measures relevance against human labels under a defined corpus and metric convention. Its failures can exist even when every unit and integration test passes. Conversely, a good relevance score does not excuse incorrect authorization or corrupted evidence records. These dimensions complement each other.

## Avoid mirrored tests

A test that reimplements the same scoring loop line for line can repeat the implementation's mistake. Prefer independently calculated examples and properties. For example, document A with queue in title and content should score five; duplicating queue in the content should not change that score. A permutation property should preserve ordered identifiers. These expectations come from the contract and arithmetic, not from copying the function into the test.

For persistence tests, assert meaningful relations and values. Check that tool calls belong to the created run, the recorded window matches the query contract, modelConfidence remains null and approval remains pending. A snapshot of a huge JSON object can be useful, but reviewers should understand which fields are semantically essential and which timestamps or identifiers are incidental.

## Negative cases are first-class evidence

Test unauthorized callers, missing incidents, no relevant runbooks, stop-word-only queries, below-threshold matches and nonpositive bounds. For no-match investigations, inspect the recommendation and model metadata rather than merely expecting an empty list. For unauthorized callers, compare record counts or captured writes before and after. For missing incidents, ensure no fabricated evidence is created.

Add a malformed or unexpected knowledge document exercise in a disposable harness. Decide whether null fields are possible under the actual domain schema and validation. Do not add hypothetical null tests to a nonnullable contract without explaining how such values could enter. Useful defensive tests begin with a plausible boundary, not arbitrary impossible objects.

## Evidence report

Write a matrix of claims and the narrowest test that supports each. Mark every test as proposed, executed or inspected historical evidence. A test described in an old VERIFICATION file is not a current execution result. Record actual commands and outcomes when you run a new isolated lab. If a full application suite is not run, state that plainly and explain which teaching claims remain supported by source inspection and the model.

The final learning artifact should let another developer reproduce your conclusions without trusting a broad “all good” statement. Accurate limits improve the report because they show where a future change needs additional validation.

[Index](README.md) · [Answers](answers/04-lab-and-evidence.md)
