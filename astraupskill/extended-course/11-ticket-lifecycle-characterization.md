# 11. Characterize the ticket state machine before tightening it

OperationsService contains a small IsAllowedTicketTransition helper. It permits a transition to the same state, rejects moves out of Closed to a different state, allows Resolved to move to Closed or InProgress, and permits other transitions under its default branch. UpdateTicketStatusAsync parses the requested status with a fallback to the current value, updates timestamps and version, adds audit records and saves.

## Current behavior versus desired workflow

A support product might want a stricter transition graph, but the first task is to record the current graph. Enumerate the actual TicketStatus values from the domain and build an action matrix. Do not invent enum members from a generic help-desk model. For each pair, state whether the helper permits it and what audit/timestamp side effects the service produces.

Same-state updates are particularly instructive. They can pass the helper and still update UpdatedAt, increment Version and add StatusChanged audit records saying the state changed from itself to itself. Decide whether that is intended command history or noisy no-op behavior. Characterize it before proposing a no-op optimization, because clients may rely on version advancement or audit traces.

## Invalid status fallback

ParseOrDefault uses Enum.TryParse and falls back to the current state when parsing fails. A malformed string can therefore become a same-state update at this service boundary rather than an explicit validation error. Inspect request validation to determine which malformed values can arrive through HTTP. Direct service tests and controller tests can have different accepted input domains.

Also investigate numeric enum strings. Enum.TryParse can have behaviors that differ from a strict named-value allowlist, depending on the enum and input. An isolated .NET test should establish the actual parsing result and whether the parsed value is defined. Do not assume parsing success always means membership in the intended business state set.

## ResolvedAt semantics

The service assigns the current time when the next state is Resolved or Closed, otherwise it retains the prior ResolvedAt value. Reopening a resolved ticket to InProgress can therefore retain a historical resolution timestamp in the inspected method. That may be useful history or confusing current-state metadata depending on the product definition.

Create a trace: New to Resolved, Resolved to InProgress, InProgress to Resolved, Resolved to Closed. Record Status, UpdatedAt, ResolvedAt and Version after each command using a proposed controlled clock. Decide whether ResolvedAt means first resolution, latest resolution or current closure boundary. The current assignment pattern does not implement all those meanings simultaneously.

## Concurrency token

[The DbContext configuration](../../src/Infrastructure/Persistence/NetAiOpsDbContext.cs) marks Ticket.Version as a concurrency token. The service increments it on several mutations. Two contexts that load the same version can conflict when saving under a relational provider. A model attribute alone does not prove the API translates the resulting exception into a useful client response; inspect middleware and test the route separately.

Design a two-context test in a disposable relational database. Load the ticket twice, update different states, save one and then the other. Assert the conflict behavior and audit rollback under the configured provider. EF InMemory tests can help with object construction but should not be presented as proof of the relational concurrency contract.

## Exercise

Submit the transition matrix, same-state behavior, invalid-input cases, resolution-time trace and concurrency test plan. Clearly separate characterization from a proposed stricter workflow. A strong answer can explain why changing a seemingly simple status helper affects audit history, versioning and client recovery.

[Index](README.md) · [Review notes](answers/06-ticket-and-visibility.md)
