# 18. Cancellation describes a request, not automatically a rollback

The investigation method accepts a CancellationToken and passes it to database queries and SaveChangesAsync. It also performs in-memory retrieval and constructs an evidence graph before saving. A cancellation can occur at different phases, and its meaning depends on which work has already completed. This chapter develops a precise cancellation test plan.

## Before durable writes

If cancellation is observed during an early query, the method may exit before adding the new graph. If it is observed after objects are added to the DbContext but before a successful SaveChanges, those objects may remain tracked in that context even though no durable commit is established. Context lifetime and disposal determine whether a later operation could accidentally reuse them.

The ordinary request scope may dispose the context, but a test that reuses one context across several service calls can behave differently. Isolate each command's context when testing request behavior, or explicitly inspect and clear tracked state only within the test's defined contract. Do not mistake an in-memory tracked object for a committed database row.

## During synchronous retrieval

LexicalRetriever.Search does not accept the cancellation token in its signature. Once the corpus is loaded, a long in-memory scoring pass does not explicitly check cancellation in that function. This is a current boundary. For a small corpus it may be acceptable; for a large corpus it can delay request cancellation responsiveness.

A proposed cancellation-aware search needs to define where checks occur and what callers observe. It should not return a partial list without labeling it as partial if the normal contract promises a complete topK over the supplied corpus. Throwing cancellation can be clearer, but benchmark overhead and test the behavior with controlled large fixtures.

## Save and response loss

Cancellation near SaveChanges is harder to interpret from the caller alone. A lost or canceled response does not automatically prove no records committed. A robust recovery design uses a command identity or a durable read to determine whether a run exists. Repeating investigation may create a second run if the first committed and there is no deduplication contract.

Design a test harness that can distinguish cancellation before save, a simulated failure during save and response loss after successful save. The database adapter determines which outcomes can be known. A fake that simply throws before writing cannot establish every real cancellation race.

## Related records

The method adds an AgentRun with tool calls, a HumanApproval, incident changes and an audit event before one SaveChanges call. A relational test should inspect whether the intended graph is atomic under the configured provider. If a constraint failure occurs on one part, verify the durable state of every part rather than checking only that the service threw.

EF InMemory can preserve object relationships for many tests but does not provide the same relational transaction semantics. Use it to inspect construction and authorization ordering, and use a disposable relational provider for atomicity. A report should state which provider produced the evidence.

## Exercise

Create a phase table with cancellation point, tracked state, durable state knowledge, caller observation and safe recovery. Include a second browser request after an unknown outcome. Propose a command receipt if the product wants retries to recover the same run rather than start another. A strong answer distinguishes canceled waiting, canceled local work and confirmed absence of durable effects.

[Index](README.md) · [Review notes](answers/09-cancellation-and-graphs.md)
