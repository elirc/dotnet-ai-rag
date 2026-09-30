# 29. Keep evidence current as the application evolves

Netairag contains several generations of learning material. The older Phase 1 guide, broader docs2.0 tracks, Astra starter and this extension can remain useful if their scope and historical claims are clear. Maintenance should connect learners to current source without rewriting old execution reports as if they ran today.

## Build a claim map

List the symbols supporting major lessons: LexicalRetriever.Search, RunIncidentInvestigationAsync, ticket transition helpers, RequestContext, OutboxWorker and the DbContext relationships. For each, record the claim, inspection date or hash, supporting test and known limits. A source change triggers semantic review, not automatic replacement of all prose.

If the outbox later gains a real delivery adapter, update the current-behavior lesson and preserve the earlier bookkeeping limitation as historical context. If approval decision endpoints are added, move only the implemented parts from proposal to current capability. If a model call is introduced, retain mode-specific descriptions so deterministic runs do not inherit model metadata accidentally.

## Update tests with intent

A changed weight should prompt both exact arithmetic updates under an approved contract and relevance reevaluation. A changed tokenizer needs punctuation, Unicode and synonym-case review. A new document visibility filter needs authorization fixtures and evaluation over the eligible corpus. A new payload version needs historical-reader tests.

Do not change expected outputs simply because the implementation changed. First decide whether the behavior change is intended and supported by evidence. A characterization test can preserve the old behavior as a regression explanation, while a new contract test establishes the desired result. This makes future reviews easier to understand.

## Execution evidence

Record exact commands and outcomes for new runs. The isolated C# harness supports its eighteen named assertions in the authoring environment. A future learner may run it under another compatible SDK and should record that separately. The Python model supports its own limited fixture. Neither result should be copied into a statement that all API or database tests passed.

For a documentation-only link change, a link check is sufficient. For a runnable sample change, execute or compile that sample. For a changed persistence claim, inspect and test the relevant relational boundary. Verification should match the claim and avoid unnecessary broad suite runs that obscure the useful evidence.

## Review a proposed pull request

Imagine a patch replaces interpolated outbox JSON with serializer-based construction. A good description names the trigger: accepted service names containing quotes or backslashes can produce malformed payloads. It states that payloads now round-trip those values and preserve null semantics. It lists focused parser/consumer tests and notes that the worker's delivery behavior is unchanged if that remains true.

Another patch adds document-version snapshots. Its description should explain how a later runbook edit previously changed what an old investigation resolved to, and how the new evidence bundle retains the reviewed content. Include migration treatment of legacy runs with missing versions. The reviewer should not need the conversation history to understand the final change.

## Exercise

Submit a claim map for six topics and two sample change descriptions. Include one historical claim that must remain dated and one proposal that has not been implemented. A strong maintenance plan keeps the course accurate and navigable without turning every source edit into a complete rewrite.

[Index](README.md) · [Review notes](answers/13-capstone-and-maintenance.md)
