# Phase 3 Journal: Knowledge Base And RAG

## Planning

Phase 3 adds document ingestion and grounded answers. The teaching goal is separating ingestion, retrieval, and answer generation so each piece can be tested independently.

## Implementation Plan

- Add knowledge document CRUD.
- Add `.md` and `.txt` upload handling.
- Add chunking and mock embeddings.
- Add hybrid retrieval.
- Add `POST /api/rag/query`.
- Add RAG assistant UI with citations.

## Retrospective Template

After implementation, document how chunk size was selected, why retrieval returns reasons, and how the mock embedding provider remains compatible with a real provider.
