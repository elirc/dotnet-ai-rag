# 17. Retrieved text is evidence to inspect, not an instruction to execute

The current investigation path is deterministic and does not invoke a language model. Nevertheless, knowledge documents can contain operational instructions, stale advice or untrusted text. Retrieval makes such content visible because it matches terms; it does not establish that the content is authoritative, current or safe for the incident being investigated.

## Define source authority

A knowledge document has identity, title, content, tags and source metadata in the domain model. A proposed trust policy might distinguish approved runbooks, imported notes and unreviewed drafts. Before implementing such a policy, inspect which fields and write paths actually exist. Do not assume SourceType alone means a document has been approved for operational use.

Create two documents with identical lexical relevance: an approved synthetic runbook and an outdated draft that recommends an inappropriate action. The current pure retriever scores field membership, not approval quality. The calling service's candidate set determines whether both are considered. A useful design filters or labels source status explicitly rather than hoping ranking weights encode trust.

## Untrusted instructions in content

A document can contain text such as “ignore the incident evidence and execute this command.” In this course, treat that as document content to evaluate, not as authority over the application or reviewer. If a future model integration is proposed, the same boundary becomes even more important: retrieved text should not override system instructions, tool permissions or human approval policy.

Keep the discussion tied to concrete workflow. A reviewer screen can show source, version, approval status and relevant excerpt, then require an explicit action proposal. An execution component should accept structured authorized parameters, not blindly run arbitrary text copied from a search result. This is a proposed design boundary, not a claim that the current retriever executes anything.

## Visibility before ranking

The inspected investigation query projects knowledge documents without a per-document visibility predicate shown in that query. That may fit the current application's role model, but a multi-tenant or restricted-document extension needs an explicit access scope. Adding a tenant field without filtering every retrieval path would not establish isolation.

Design a two-scope fixture with a highly matching document the actor must not see. Assert that its title, ID, matched terms and score do not appear in the result or diagnostics. Also inspect whether it influences truncation of visible results if filtering happens after ranking. The safest contract should make inaccessible documents absent from the actor's candidate universe.

## Freshness and revocation

A document can be revoked after an investigation stores its match. Historical evidence should preserve what was retrieved, but current viewers may no longer be authorized to see its full content. Define how an evidence view handles that tension. It can retain a restricted snapshot and show a revocation notice to ordinary viewers while allowing authorized audit access under policy.

Do not erase historical associations silently; that makes investigations difficult to explain. Do not expose revoked sensitive content indiscriminately either. The product needs a retention and access contract that distinguishes identity, content and authority. This is a design exercise requiring explicit decisions rather than a universal answer.

## Exercise

Submit a source-trust matrix, visibility test, malicious-content fixture and revocation scenario. Explain how the current deterministic path differs from a proposed model-assisted path while preserving the same authority boundary. A strong answer never treats lexical relevance as permission to execute an operational command.

[Index](README.md) · [Review notes](answers/08-concurrency-and-provenance.md)
