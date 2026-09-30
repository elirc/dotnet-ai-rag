# Review notes: concurrent runs, scale and trust

Two investigation calls can create separate runs and approvals while writing one mutable incident candidate. A concurrency token helps only according to how writers compare and advance it. The investigation path should be tested against another investigation and against a timeline update in separate contexts. A slower earlier run can finish later, so a selected-run policy should use explicit identity and ordering rather than accidental last save.

A request receipt can distinguish recovery of a lost response from a deliberate run-again action. Its payload equivalence should account for the incident snapshot. Historical runs can coexist, but the UI should identify which run and evidence version support the currently displayed candidate.

topK limits output, while the inspected service loads the full projected knowledge corpus and the retriever scores and sorts candidates. A pre-tokenized index or bounded selection structure can improve measured cost, but it needs update, version and tie-order contracts. A search-service rewrite can change tokenization and relevance, requiring differential fixtures and fresh evaluation.

Source trust and document visibility are separate from lexical score. A highly matching document can be obsolete, unauthorized or contain instructions that should not be executed. Retrieved text remains evidence for review. A future model integration must preserve the application's authority boundary rather than letting document text override tool permissions or approval policy.

Revocation creates a tension between historical reproducibility and current access. A proposed retained snapshot can be restricted, while ordinary viewers see a revocation notice. Silently erasing all associations harms explanation; exposing sensitive historical content to everyone harms access control. The product must define identity, content retention and authority explicitly.

[Index](../README.md)
