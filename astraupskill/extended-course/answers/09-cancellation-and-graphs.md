# Review notes: cancellation and evidence graphs

Cancellation before save can leave tracked objects without durable rows. A request-scoped context and a reused test context are different environments. The pure retriever has no cancellation-token parameter, so synchronous scoring does not explicitly observe cancellation. A proposed cancellation-aware search should not return an unlabeled partial topK as if it covered the full corpus.

A lost or canceled response near commit does not prove absence. Recovery should use a stable command identity or durable association. Repeating investigation without such a contract can create another run. Tests need separate fault points for early query cancellation, save failure and response loss after successful save.

The graph includes configured relational links and IDs embedded in JSON. AgentToolCall belongs to AgentRun through a configured relationship; a document ID in output JSON does not necessarily have database-enforced referential integrity. The inspected HumanApproval entity lacks a direct AgentRunId field, so a proposal requiring that navigation should add it explicitly rather than infer it from copied JSON.

Retention can use live references, snapshots or tombstones, each with different reproducibility and access costs. A read-only integrity checker should report unresolved historical references without silently deleting evidence. Payload versioning should preserve unknown metadata as unknown, especially model-use and confidence fields. One malformed historical record should have a bounded display failure rather than breaking every recent-run view.

[Index](../README.md)
