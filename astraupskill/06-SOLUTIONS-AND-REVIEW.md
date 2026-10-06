# Answers and review criteria

## 1. Scores and order

A scores 5 (title 4 + content 1), B scores 3 (tags), C scores 1 (content). A and B pass the
default threshold of 3 and appear in that order; C is excluded because 1 < 3. With
`topK: 1`, only A remains. The result is explained by field membership, not term frequency:
repeating `queue` ten times in C's content still contributes one point for that field,
because each field is turned into a set
([`LexicalRetriever.cs:37-39`](../src/Application/Retrieval/LexicalRetriever.cs#L37-L39)).

## 2. Ties and input order

The identifier comparison
([`LexicalRetriever.cs:30`](../src/Application/Retrieval/LexicalRetriever.cs#L30)) resolves
the remaining tie, so reversing input order must not reverse the result. LINQ's `OrderBy` is
stable, but a stable sort only *preserves the supplied order* among equal keys. If the
source order changes (a database returning rows without `ORDER BY` is the usual case), so
does the output. The explicit final key is therefore part of the observable contract, not
decoration.

## 3. The truthful no-match investigation

Both searches still ran, so the `AgentRun` has two tool children, `TotalToolCalls = 2`,
zero tokens and cost, `modelUsed: false`, `modelConfidence: null`, and
`runbookMatches: []`. The recommendation is the line-349 text: *"No relevant runbook
matched. Continue with manual investigation and do not infer a mitigation from absent
evidence."* The incident candidate is the line-430 text, its status `PendingApproval`
(line 431). A `HumanApproval` is still created as `Pending`, and one `AgentRunCreated`
audit row is added. No-match does not let the code skip human review or claim a root
cause.

## 4. The log window

`evidenceCount` is 25; `sampleLogIds` holds 20 (the `Take(20)` at
[`OperationsService.cs:329`](../src/Infrastructure/Operations/OperationsService.cs#L329)).
The two out-of-window rows affect neither. For determinism, place in-window logs well
inside the window (`detectedAt ± minutes`), the "before" log at `detectedAt - 3h`, and the
"after" log at `UtcNow + 1h`. Without an injectable clock you can't put a row exactly on an
edge, and you shouldn't try: the upper bound is captured at run time (line 320). A clock
abstraction (`TimeProvider` in .NET 8) would make exact edge tests possible; that is a
separate, bounded change, so propose it rather than smuggling it into a test PR.

## 5. What can a Viewer change?

Nothing, at either layer. Over HTTP, the `CanInvestigate` policy
([`IncidentsController.cs:43`](../src/Api/Controllers/IncidentsController.cs#L43)) returns
403 before the service is called. Calling the service directly, the role check at
[lines 307-310](../src/Infrastructure/Operations/OperationsService.cs#L307-L310) throws
before any query or `Add`, so there is no `AgentRun` or `HumanApproval`. The existing test
asserts exactly that (lines 135-137).

## 6. Who reads the internal notes?

The Viewer receives the whole ticket, **including every internal note**. `CanReadTicket`
returns true for the assignee (line 519), and `ToTicketDetail` maps *all* comments with no
`IsInternal` filter (line 601). The ticket's own "Ticket created." comment is itself
internal (lines 103-111). No test in `tests/` mentions `IsInternal`. A mid-level fix keeps
`CanReadTicket` as the *object* policy and adds a separate *field* policy (internal notes
only for `CanMutateSupportWork`), with a test for each role. Extended lesson 12 develops
it.

## 7. Two investigations at once

**Two investigations:** both succeed. Each adds its own `AgentRun`, tool calls, approval,
and audit row. Both update the incident with `WHERE version = N`, and since neither
increments `Version`, both updates match. The candidate text from whichever save commits
*last* survives, even if that run started first.
**Investigation vs. timeline edit:** if the timeline save commits *between* the
investigation's load (line 312) and its save (line 434), it moves `Version` to N+1
(line 299). The investigation's update still says `WHERE version = N`, matches zero
rows, and EF throws `DbUpdateConcurrencyException`. The middleware maps that to **409**
([`ExceptionHandlingMiddleware.cs:20`](../src/Api/Middleware/ExceptionHandlingMiddleware.cs#L20)),
and because it's one `SaveChangesAsync`, the run and approval are not saved either.
Both answers are predictions from configuration. Say so in your write-up, and name the
relational test that would confirm them.

## Review criteria for any change to this command

Reject a change that:

- adds a hard-coded confidence value or labels lexical matching as model inference;
- changes weights, threshold, or tokenization **without** a new evaluation case that shows
  the intended effect (chapter 4 showed the current tests won't catch it for you);
- claims relational rollback, isolation, or performance based only on the InMemory fixture;
- builds JSON by string interpolation from user-controlled text;
- adds an approval "decision" without linking the approval to the run it decides
  (`HumanApproval` has no `AgentRunId` today).

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
