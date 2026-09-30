# Answers and review criteria

For the first exercise, A scores five, B scores three, and C scores one. A and B pass the default threshold and appear in that order. With topK one, only A remains. The result is explained by field membership, not term frequency: repeating queue ten times in C's content still contributes one point for that field.

For the second exercise, the identifier comparison resolves the remaining tie. Reversing input order must not reverse the result. A stable sorting implementation only preserves the supplied order among equal keys; it cannot make an unstable source enumeration deterministic. The explicit final key is therefore part of the observable contract.

For the no-match investigation, both searches still occurred. The AgentRun has two tool children, zero model tokens and cost, modelUsed false, and null modelConfidence. The recommendation asks for manual investigation and the approval remains pending. No-match does not grant permission to skip human review or claim a root cause.

For the log fixture, the expected match count is twenty-five and the saved identifier sample contains twenty. Out-of-window rows must affect neither. A clock abstraction would make exact endpoint tests clearer if introduced in a future bounded change; this patch uses an actual captured command start time and its tests assert permissible ordering.

The viewer request throws before persistence, so there is no AgentRun or HumanApproval. Review proposed changes for regressions at these boundaries. Reject a solution that adds a hardcoded confidence value or labels lexical matching as model inference. Also reject claims of relational rollback based solely on this InMemory test fixture.

## Source excerpt

From [tests/UnitTests/LexicalRetrieverTests.cs](../tests/UnitTests/LexicalRetrieverTests.cs).

```cs
    [Fact]
    public void Unrelated_or_stop_word_only_queries_return_no_match()
    {
        LexicalRetriever.Search([Billing, Auth], "printer toner tray").Should().BeEmpty();
        LexicalRetriever.Search([Billing, Auth], "investigate the service incident").Should().BeEmpty();
    }

    [Fact]
    public async Task Checked_in_evaluation_cases_are_loaded_and_pass()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "sample-eval-cases", "retrieval.json");
        await using var stream = File.OpenRead(path);
        var cases = await JsonSerializer.DeserializeAsync<EvaluationCase[]>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        cases.Should().NotBeNullOrEmpty();
        foreach (var evaluation in cases!)
        {
            var matches = LexicalRetriever.Search([Billing, Auth], evaluation.Query);
            if (evaluation.ExpectedMatches is not null)
            {
                matches.Should().HaveCount(evaluation.ExpectedMatches.Value, evaluation.Id);
            }
            if (evaluation.ExpectedTopTitle is not null)
            {
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
