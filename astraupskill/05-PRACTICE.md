# Exercises in ranking and evidence inspection

Exercise one: predict scores for the query queue. Document A has queue in its title and content, document B only in tags, and document C only in content. Use the production weights and threshold. State the returned order and explain which document is excluded. Then repeat with topK equal to one. Write the expected identifiers, not just the number of results.

Exercise two: give two documents the same score and title but different identifiers. Reverse their input order. Predict the output before reading the tie-break expression. Explain why a stable sort alone would not provide the same guarantee when the source enumeration changes.

Exercise three: design an incident investigation with no relevant knowledge document. List the persisted records and the output values you expect. Pay attention to modelUsed, modelConfidence, tool count, approval status, and the recommendation. Absence of a runbook is a valid search outcome and should not be translated into a fabricated mitigation.

Exercise four: propose a service test containing twenty-five matching logs, one log before the lower bound, and one log after the upper bound. Predict the total count and sample size separately. Specify how you would make time boundaries deterministic instead of relying on a sleep.

Finally, describe what an unauthorized viewer request may change. Compare your answer with the existing authorization test. Keep proposed fixture changes local and isolated; none of these exercises requires a provider credential, a deployed incident, or access to an existing operational database.

## Source excerpt

From [tests/UnitTests/LexicalRetrieverTests.cs](../tests/UnitTests/LexicalRetrieverTests.cs).

```cs
    [Fact]
    public void Ranking_is_stable_bounded_and_uses_title_then_id_for_ties()
    {
        var b = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000011"), "B Runbook", "queue", "queue");
        var a = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000012"), "A Runbook", "queue", "queue");
        var laterId = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000014"), "Same Runbook", "cache", "cache");
        var earlierId = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000013"), "Same Runbook", "cache", "cache");
        var results = LexicalRetriever.Search([b, a, Billing], "queue", topK: 1);
        results.Should().ContainSingle();
        results[0].Title.Should().Be("A Runbook");
        LexicalRetriever.Search([laterId, earlierId], "cache")
            .Select(x => x.DocumentId).Should().Equal(earlierId.Id, laterId.Id);
    }

    [Fact]
    public void Unrelated_or_stop_word_only_queries_return_no_match()
    {
        LexicalRetriever.Search([Billing, Auth], "printer toner tray").Should().BeEmpty();
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
