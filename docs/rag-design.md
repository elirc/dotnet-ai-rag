# RAG Design

> **Implementation status (verified 2026-10-06):** none of the planned interfaces below (`IEmbeddingClient`, `ILlmClient`, `IDocumentChunker`, `IRetrievalService`) exists in `src/`. There are no embeddings, chunks, vectors, or answer generation. What *does* exist is a deterministic lexical ranker, `src/Application/Retrieval/LexicalRetriever.cs` (title/tag/content weights 4/3/1, threshold 3, top 3), used by the incident investigation. It records `heuristicScore`, not model confidence. `astraupskill/02-CONCEPTS.md` explains it. Read the rest of this file as a design plan.

RAG begins in Phase 3. The design goal is to keep retrieval, answer generation, and citation formatting separate.

## Planned Interfaces

- `IEmbeddingClient`: creates vectors from text.
- `ILlmClient`: produces answers from retrieved context.
- `IDocumentChunker`: normalizes and splits documents.
- `IRetrievalService`: performs hybrid keyword and vector-like retrieval.

## Mock Embeddings

The default provider should be deterministic so evals can run without paid APIs. A good first implementation is hashed word buckets:

1. Normalize text.
2. Hash tokens into fixed vector buckets.
3. Normalize vector length.
4. Store the resulting float array.

## Retrieval

The retrieval service should combine:

- Keyword search over title, tags, and chunk text
- Mock vector similarity
- Metadata filters such as service and document type
- Reranking stub for later model-based reranking

## Answering

The RAG endpoint should return:

- Answer
- Citations
- Retrieval results and scores
- Reasons explaining why chunks were selected

The answer generator should refuse to invent facts when retrieved chunks are weak.
