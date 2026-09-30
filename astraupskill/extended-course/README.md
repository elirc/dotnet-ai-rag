# Netairag retrieval and evidence workshop

Continue from the [original starter course](../README.md). Read the source-linked lessons, make predictions before opening answers, run the isolated teaching lab, and complete an independent capstone. Proposed changes are explicitly labeled; application code is unchanged. Historical verification claims are not current execution evidence.

## Learning route

- [1. Explain a retrieval result without invoking a model](01-ranking-by-hand.md)
- [2. Bounded results must still be deterministic](02-thresholds-and-ties.md)
- [3. Measure relevance separately from implementation correctness](03-evaluation-dataset.md)
- [4. A bounded log sample is not the full incident](04-evidence-window.md)
- [5. A retrieved candidate is not an authorized mitigation](05-provenance-and-human-approval.md)
- [6. Match the test to the claim](06-test-layers-and-course-map.md)
- [7. Run a transparent ranking laboratory](07-retrieval-lab-and-capstone.md)
- [8. A stored approval is not a complete approval workflow](08-approval-api-boundary.md)
- [9. Read what the outbox worker actually processes](09-outbox-worker-semantics.md)
- [10. Serialize data rather than assembling a JSON-shaped string](10-json-and-payload-integrity.md)
- [11. Characterize the ticket state machine before tightening it](11-ticket-lifecycle-characterization.md)
- [12. Read access and field visibility are separate policies](12-object-access-and-internal-notes.md)
- [13. Compile the real retriever in a small harness](13-actual-source-retrieval-lab.md)
- [14. Two investigations can produce two valid runs and one latest candidate](14-concurrent-investigations.md)
- [15. Bounded lists need honest completeness signals](15-query-pagination-and-completeness.md)
- [16. A top-three result can still scan the whole corpus](16-corpus-scale-and-index-design.md)
- [17. Retrieved text is evidence to inspect, not an instruction to execute](17-document-trust-and-visibility.md)
- [18. Cancellation describes a request, not automatically a rollback](18-cancellation-and-save-boundaries.md)
- [19. A JSON identifier is not necessarily a relational link](19-evidence-graph-integrity.md)
- [20. An independent ranking casebook](20-ranking-casebook.md)
- [21. Distinguish observed time from a convenient default](21-event-time-and-provenance.md)
- [22. A comma-separated field can lose structure](22-tags-and-normalization.md)
- [23. Names containing AI do not establish model behavior](23-ai-labels-and-evidence-contracts.md)
- [24. Test the saved investigation graph with a real relational boundary](24-relational-investigation-campaign.md)
- [25. Investigate evidence that looks more certain than it is](25-incident-evidence-dossiers.md)
- [26. Propose one retrieval improvement with a measurable tradeoff](26-retrieval-change-proposal.md)
- [27. Design an investigation that can be reconstructed later](27-versioned-evidence-capstone.md)
- [28. Evaluate when the retriever should say nothing useful matched](28-negative-evaluation-and-abstention.md)
- [29. Keep evidence current as the application evolves](29-maintaining-a-source-grounded-course.md)
- [30. Calculate a complete bounded-evidence fixture](30-log-window-workbook.md)
- [31. Calculate aggregate metrics without hiding difficult queries](31-worked-evaluation-accounting.md)
- [32. Review a recommendation with another engineer](32-evidence-review-workshop.md)

## Separate review material

- [Answers: lexical mechanics](answers/01-ranking.md)
- [Answers: relevance evaluation](answers/02-evaluation.md)
- [Answers: evidence and authority](answers/03-evidence-and-approval.md)
- [Answers: tests and capstone](answers/04-lab-and-evidence.md)
- [Review notes: approval, outbox and representation](answers/05-approval-outbox-and-json.md)
- [Review notes: ticket behavior and visibility](answers/06-ticket-and-visibility.md)
- [Review notes: runtime and query boundaries](answers/07-query-and-runtime.md)
- [Review notes: concurrent runs, scale and trust](answers/08-concurrency-and-provenance.md)
- [Review notes: cancellation and evidence graphs](answers/09-cancellation-and-graphs.md)
- [Answers: independent ranking casebook](answers/10-ranking-casebook.md)
- [Review notes: time, tags and AI metadata](answers/11-time-tags-and-metadata.md)
- [Review notes: relational campaign and incident dossiers](answers/12-integration-and-incidents.md)
- [Review notes: capstones, abstention and maintenance](answers/13-capstone-and-maintenance.md)
- [Answers: log and evaluation calculations](answers/14-final-calculations.md)

The Python lab in `labs` uses synthetic state and no network or application database. Its guide describes the model limitations. Current execution and preservation evidence is recorded in the workspace nine-project expansion report.
