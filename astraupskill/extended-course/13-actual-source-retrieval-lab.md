# 13. Compile the real retriever in a small harness

The [isolated C# project](labs/actual-source/RetrievalContract.csproj) compiles the repository's actual LexicalRetriever source together with a short assertion program. It has no external package references and does not start the API or connect to a database. The lab targets the installed .NET 10 SDK for isolation; it is not a change to the application's own target framework.

## Why add this after the Python model?

The Python model makes arithmetic transparent, but it uses a simplified identifier and comparison domain. The C# harness executes the actual generated regex, ordinal case-insensitive title comparison and Guid tie-break. It therefore closes a specific evidence gap. It still does not exercise EF query translation, authorization, persisted evidence or live operational relevance.

Run `dotnet run --project labs/actual-source/RetrievalContract.csproj` from this course directory with a compatible SDK. Build outputs belong to the isolated lab. The project includes the application source by a relative path, so it compiles what is currently present rather than a silently diverging reimplementation. If the source changes, review the expected contract before accepting a new result.

## Independent expected values

The fixture's queue scores are written as five and three from the field-weight arithmetic. The assertions do not call the scorer again to calculate expected scores. The program checks repetition invariance, stop-word-only queries, nonpositive bounds, topK, punctuation splitting, matched-term order, repeated field occurrences and actual Guid tie ordering for selected values.

The permutation loop checks every ordering of the three-document fixture. This establishes input-order independence for those documents. It does not prove the comparator is correct for every possible Unicode title or identifier. The title-case tie fixture specifically checks the intended secondary and tertiary keys in the actual runtime.

## Read a failure usefully

If a check fails after a code change, identify whether the contract changed intentionally or the implementation regressed. A renamed field may require a compile update; a changed weight may require a product decision and evaluation report. Do not automatically edit expected values until the test passes. The independent arithmetic is useful precisely because it can challenge implementation behavior.

Capture the source hash, SDK version, command and output. A successful run supports the named pure-function assertions. It should not be summarized as “the investigation system passed” because that would include persistence, authority and evidence semantics outside the harness.

## Mutation extension

Copy the lab and source into a disposable directory if you want to mutate the implementation. Change the threshold comparison, remove the final tie-break or alter query deduplication one at a time. Identify which assertion fails and why. Do not edit the real application source as a shortcut for a learning exercise.

If a mutation survives, inspect whether the fixture actually reaches the changed behavior. Removing the ID tie-break will not be detected by documents with different scores or titles. A useful mutant case creates equality in all earlier keys. This is a general test-design skill: arrange the fixture so the targeted rule is decisive.

## Deliverable

Submit the unmodified run result, one mutant counterexample and a list of remaining evidence layers. Include a plan for a relational service test and a human-labeled relevance evaluation. The lab is successful when the learner can explain why actual-source execution is stronger than a parallel model for runtime details while still narrower than an end-to-end guarantee.

[Index](README.md) · [Review notes](answers/07-query-and-runtime.md)
