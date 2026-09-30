# Verification scope and reproducible evidence

Accepted checks below are generated from completed command records. Their logs and machine-readable results define the tested scope. Do not infer that every frontend, deployment, or provider workflow has been exercised from a successful unit suite. The change is centered on the .NET investigation service and its pure retrieval component.

The unit suite includes existing project tests as well as the new retrieval and persistence regressions. The evaluation JSON is read from the test output directory, so its copy configuration is part of the test setup. The service fixture uses a unique InMemory database for each test, avoiding the original application database. No model provider or external mitigation endpoint is involved.

The local runtime matters for reproduction. The project targets .NET 8 and this workstation also has .NET 10 installed globally. Use the recorded .NET 8 executable and environment setting when repeating the accepted tests here. Earlier missing-runtime or incomplete-restore attempts are retained separately and are not counted as passes. A terminal exit of zero is required before a check is accepted.

Important limits remain explicit: ranking is lexical and ASCII-token based; all knowledge rows are materialized before ranking; log counts and samples use separate queries; and InMemory tests do not verify PostgreSQL isolation or query performance. These limits constrain the evidence, not the test result. A future scaling or relational-integration task should define its own acceptance criteria before changing this bounded implementation. Original source snapshots accompany the delivered changes so reviewers can inspect the exact before-and-after behavior.

## Source excerpt

From [tests/UnitTests/LexicalRetrieverTests.cs](../tests/UnitTests/LexicalRetrieverTests.cs).

```cs
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
                matches.Should().NotBeEmpty(evaluation.Id);
                matches[0].Title.Should().Be(evaluation.ExpectedTopTitle, evaluation.Id);
            }
        }
    }

    private sealed record EvaluationCase(
        string Id,
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)

## Recorded command evidence

The recorded test runs passed **11 tests**. The commands below define the verified scope; counts refer to tests, not commands.

| Check | Exact command arguments | Exit | Evidence |
|---|---|---:|---|
| `astra-netairag-tests-r2` | `["C:/Users/Owner/Desktop/AstraOrganized2/astra-remaining-improvements/toolchains/dotnet8/dotnet.exe", "test", "tests/UnitTests/NetAiOps.UnitTests.csproj", "--no-build", "--no-restore", "--logger", "trx", "--verbosity", "minimal"]` | 0 | [record](evidence/astra-netairag-tests-r2.json), [log](evidence/astra-netairag-tests-r2.log) |
| `astra-netairag-build-r3` | `["C:/Users/Owner/Desktop/AstraOrganized2/astra-remaining-improvements/toolchains/dotnet8/dotnet.exe", "build", "NetAiOps.sln", "--nologo", "--verbosity", "minimal", "/m:1", "/p:RestoreDisableParallel=true"]` | 0 | [record](evidence/astra-netairag-build-r3.json), [log](evidence/astra-netairag-build-r3.log) |

[Machine-readable results 1](evidence/regression.trx).
