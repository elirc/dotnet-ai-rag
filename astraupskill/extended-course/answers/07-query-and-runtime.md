# Review notes: runtime and query boundaries

The C# harness compiles the actual retriever and checks eighteen assertions in the authoring run. It covers independent field arithmetic, selected tokenization behavior, nonpositive bounds, repetition, matched-term order, actual title/Guid ties and all permutations of a three-document fixture. It does not run the API, an EF provider or a model call.

The Python model and C# harness have different scopes. The former is a transparent ASCII/numeric-ID teaching model; the latter exercises actual .NET behavior for its fixture. Neither establishes operational relevance. Human labels and a corpus-versioned evaluation are still needed for that claim.

Offset paging over inserted leading rows can repeat or skip rows relative to an earlier page. A unique final ordering key makes ties explicit but does not create a snapshot. Count and page reads can observe different populations. Fixed Take limits on run and approval lists do not imply complete history, so UI wording should indicate recent or bounded results.

A cursor proposal should define ordering, filter identity and completeness semantics. Evidence capture may need retained selected IDs or a snapshot, while ordinary browsing can tolerate changing totals under a clear contract. An empty page or truncated list is not proof that no relevant record exists anywhere.

[Index](../README.md)
