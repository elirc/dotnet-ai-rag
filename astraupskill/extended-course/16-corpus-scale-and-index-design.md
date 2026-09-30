# 16. A top-three result can still scan the whole corpus

RunIncidentInvestigationAsync loads knowledge documents into memory, projecting ID, title, tags and content, then calls LexicalRetriever.Search with topK three. The retriever scores supplied documents, filters and sorts before taking the bound. Returning only three results does not mean the upstream work examines only three documents.

## Trace the cost

Let the corpus contain N documents with total text size B. Loading all projected content incurs database transfer and memory cost related to B. Tokenizing each field and scoring query terms adds CPU work. Sorting qualifying matches adds another cost before truncation. The exact complexity and allocations depend on implementation details, but the key observation is that the result bound does not bound corpus ingestion.

Create synthetic corpora with increasing document count and content length. Measure database loading separately from pure retrieval if possible. A pure-function benchmark with preloaded documents cannot establish the cost of querying and materializing the corpus. Conversely, a database query benchmark does not measure tokenization and sorting.

## Pre-tokenization proposal

A proposed index can store token sets or postings so repeated queries do not tokenize every document from scratch. It needs an update protocol when titles, tags or content change. An index that is fast but stale can return obsolete recommendations. Define index version, source document version and how evaluation records which index was used.

If indexing is asynchronous, a newly edited runbook may not be immediately searchable. The UI and approval workflow should know that delay. A provenance record should identify the exact document version returned, not merely the latest version at display time. Performance architecture and evidence integrity are connected.

## Bounded selection

When only a small topK is needed, a bounded selection structure can avoid sorting every qualifying match. It must preserve the same total ordering: score, title comparer and ID. A performance rewrite that drops a tie-break can change results under equal scores. Use the existing independently calculated fixtures and permutation properties as regression tests.

Do not optimize based only on asymptotic notation. For a small educational corpus, a simple sort may be clearer and fast enough. Measure representative sizes and compare complexity of maintenance. The course asks for a justified proposal, not an automatic replacement with a sophisticated search engine.

## Database search is another contract

Moving scoring into SQL or a search service can change tokenization, case handling, punctuation, Unicode behavior and ranking. A similar-looking query is not necessarily equivalent to the current C# function. Create a differential fixture set and compare IDs, scores and ties. If semantics intentionally change, update evaluation labels and explanation UI accordingly.

Authorization filtering should happen before or as part of candidate selection under the desired policy. Retrieving private documents and discarding them only after ranking can affect visible result counts and potentially leak information through diagnostics. Define document visibility first, then design the index around it.

## Deliverable

Submit a cost model, benchmark plan, one index proposal and a semantic-compatibility test set. Include stale-index behavior and a source-version field in the proposed provenance. A strong answer recognizes that topK limits output, not necessarily work, and preserves explainability while improving measured cost.

[Index](README.md) · [Review notes](answers/08-concurrency-and-provenance.md)
