# Verification scope and reproducible evidence

Accepted checks below come from completed command records. Their machine-readable results
define the tested scope. Don't infer from a successful unit suite that any frontend,
deployment, or provider workflow was exercised. The change is centered on the .NET
investigation service and its pure retrieval component.

The unit suite includes the existing project tests as well as the new retrieval and
persistence regressions. The evaluation JSON is read from the test output directory, so its
copy configuration
([`NetAiOps.UnitTests.csproj:32`](../tests/UnitTests/NetAiOps.UnitTests.csproj#L32)) is
part of the test setup. The service fixture uses a unique InMemory database for each test,
never the application database. No model provider or external endpoint is involved.

**Reproducing locally.** Every project targets `net8.0` and the repo has no `global.json`.
If your machine has only a newer SDK, install the .NET 8 runtime before running the tests.
A test host that fails to start is a runner problem, not a test result (chapter 4).
Commands, from the repository root:

```
dotnet build NetAiOps.sln
dotnet test tests/UnitTests/NetAiOps.UnitTests.csproj --logger trx
```

Important limits remain explicit: ranking is lexical and ASCII-token based; all knowledge
rows are materialized before ranking; log counts and samples use separate queries; and
InMemory tests don't verify PostgreSQL isolation or query performance. These limits
constrain what the evidence shows, not the test result. Original source snapshots in
[`snapshots/`](snapshots/) let reviewers inspect the exact before-and-after behavior
(chapter 3 walks through the diff).

## Recorded command evidence (2026-09-19)

The recorded test run passed **11 tests**: 7 `[Fact]` methods plus one `[Theory]` with 4
`[InlineData]` rows, in 4 test files. Counts are test cases, not commands. The `.trx` file
confirms it: `total="11" executed="11" passed="11" failed="0"`.

| Check | Command (toolchain path generalized) | Exit | Evidence |
|---|---|---:|---|
| `astra-netairag-tests-r2` | `dotnet test tests/UnitTests/NetAiOps.UnitTests.csproj --no-build --no-restore --logger trx --verbosity minimal` | 0 | [record](evidence/astra-netairag-tests-r2.json), [results (.trx)](evidence/regression.trx) |
| `astra-netairag-build-r3` | `dotnet build NetAiOps.sln --nologo --verbosity minimal /m:1 /p:RestoreDisableParallel=true` | 0 | [record](evidence/astra-netairag-build-r3.json) |

Both runs used a workstation-local .NET 8 toolchain (`DOTNET_ROOT` pointed at it); the
exact absolute paths are in the JSON records. The plain-text `.log` files those records
name were **not committed**, so the JSON records and the `.trx` file are the only evidence
in the repo.

## Re-verification note (2026-10-06)

A static documentation pass (no `dotnet` run) re-counted test attributes in `tests/` and
got the same 11 cases. It also script-checked that every file link in chapters 01-07
resolves and that cited line ranges exist. The 11 test names in `regression.trx` match the
11 cases in the current tree one for one. That is evidence the recorded run exercised these
tests; it is not a fresh run.

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
