# 24. Test the saved investigation graph with a real relational boundary

The pure retriever and teaching models are deliberately narrow. This workshop designs a relational integration campaign for RunIncidentInvestigationAsync. It is a proposed campaign unless separately executed and recorded. Its purpose is to verify persistence, concurrency and query behavior that an in-memory model cannot establish.

## Disposable environment

Use a database instance or schema created for the test and verify its resolved connection before applying migrations or cleanup. Seed synthetic users, incidents, logs and knowledge documents. Do not point the harness at an existing operational database. Record provider, schema revision and relevant isolation settings in the test report.

Each test should have a fresh context or a clearly defined context lifetime. Tracking can make a later assertion read cached objects rather than durable database state. For persistence assertions, open a separate context or use no-tracking reads after the operation. This helps distinguish successful SaveChanges from objects merely present in memory.

## Atomic graph test

Create one authorized investigation with a known incident, three runbooks and a bounded log fixture. After saving, inspect the run, its two tool calls, pending approval, incident candidate and global audit. Compare JSON fields with independently calculated expected scores and bounds. Verify the relational tool-call association and separately inspect JSON references.

Inject a constraint failure on one late part of the graph in a disposable setup. Assert that the intended transaction semantics leave no partial durable run or approval if SaveChanges fails atomically under the provider. The exact fault mechanism should be documented and restored. A test that throws before SaveChanges does not prove rollback of a partially executed save.

## Authorization no-write test

Invoke the service as an unauthorized role. Record relevant row counts before and after. Assert that no AgentRun, AgentToolCall, HumanApproval or audit mutation was persisted and that the incident candidate remains unchanged. The controller policy is another layer; a direct service test confirms the service's own guard without pretending to test authentication middleware.

For a missing incident under an authorized context, expect the method's null result and no graph creation. For no matching runbooks, expect a completed deterministic run with explicit no-match guidance. These three cases should not share one generic “empty result” assertion because their semantics differ.

## Concurrent reads and writes

Use two contexts to investigate the same incident and to race investigation with a timeline update. Capture Version changes and exception behavior. Add a log between count and sample queries through a controlled seam if the desired consistency contract needs examination. A separate test can update a knowledge document between corpus read and evidence save to reveal provenance requirements.

These schedules require test seams or a carefully controlled database harness. Avoid arbitrary sleeps that merely hope to hit the gap. Record events proving the intended order. If the current architecture lacks a clean seam, a disposable adapter wrapper can be part of the lab design rather than a production hook.

## Query translation

Verify the actual SQL behavior for Contains, case comparisons and timestamp bounds under the configured provider. The pure tokenization function uses .NET rules, while database filters use translated query semantics. A service that works under EF InMemory can still behave differently when translated to SQL. Use exact synthetic strings and times to isolate the difference.

## Deliverable

Submit fixtures, fault points, expected durable rows and a claim/evidence table. The campaign should close specific gaps left by the pure harness. It should not claim relevance quality, external model behavior or complete security from relational persistence tests alone.

[Index](README.md) · [Review notes](answers/12-integration-and-incidents.md)
