# Deterministic ranking and truthful measurements

Lexical retrieval answers a narrow question: which stored documents contain the query's
meaningful tokens? Everything below is in one 59-line file,
[`LexicalRetriever.cs`](../src/Application/Retrieval/LexicalRetriever.cs).

## The rules, with line numbers

| Rule | Line(s) | Consequence |
| --- | --- | --- |
| Tokens are runs of `[a-z0-9]` after `ToLowerInvariant` | 54-58 | `BillingService` is **one** token (`billingservice`); `billing-api` is **two** (`billing`, `api`); `time out` is two tokens and never equals `timeout` |
| 14 stop words are removed from the *query* only | 10-14, 23 | includes domain words `incident`, `investigate`, `service`, so "investigate the service incident" has zero terms and returns nothing (test at [`LexicalRetrieverTests.cs:44-49`](../tests/UnitTests/LexicalRetrieverTests.cs#L44-L49)) |
| Query terms are `Distinct` | 23 | repeating a word in the incident text doesn't multiply its weight |
| Each document field is a *set* | 37-39 | ten repetitions of a word in content still score 1 |
| Weights: title 4, tags 3, content 1, additive | 44-46 | a term in all three fields scores 8 |
| Threshold `minimumScore = 3` (default) | 20, 27 | a content-only single-term match (score 1) is dropped |
| Order: score desc → title (case-insensitive ordinal) → id | 28-30 | fully deterministic, whatever the input order |
| `Take(topK)` after filter and sort | 31 | `topK` bounds the *output*, not the work |
| `topK <= 0` or `minimumScore <= 0` → empty | 22 | nonpositive bounds are treated as "nothing", not as "unbounded" |

These weights are an explicit heuristic, not a calibrated probability. That is why the
service saves them as `heuristicScore` and sets `modelConfidence` to `null`
([`OperationsService.cs:357-371`](../src/Infrastructure/Operations/OperationsService.cs#L357-L371)).

## Worked example: score the test fixture by hand

The service test seeds one incident and one runbook
([`OperationsServiceTests.cs:24-64`](../tests/UnitTests/OperationsServiceTests.cs#L24-L64)).
The retrieval query is `PrimaryService + Title + Description + AffectedServicesCsv`
([`OperationsService.cs:334-340`](../src/Infrastructure/Operations/OperationsService.cs#L334-L340)):

```
billing-api "blue" \ primary  Payment capture failures  Billing requests time out during capture.  payments,billing
```

Distinct query terms, in first-seen order: `billing, api, blue, primary, payment, capture,
failures, requests, time, out, during, payments`.

The runbook: title `Billing capture timeout runbook`, tags `billing,payments,timeout`,
content `Inspect payment capture latency and the billing dependency.`

| Term | Title (4) | Tags (3) | Content (1) | Score |
| --- | --- | --- | --- | --- |
| billing | ✓ | ✓ | ✓ | 8 |
| payment | | | ✓ | 1 |
| capture | ✓ | | ✓ | 5 |
| payments | | ✓ | | 3 |
| *all others* | | | | 0 |
| **total** | | | | **17** |

`matchedTerms` = `[billing, payment, capture, payments]`. Two lessons are hiding here:

1. **`timeout` never matched.** The runbook says `timeout` (title and tags, 7 points
   available); the incident says `time out`. Two words, two tokens, zero overlap.
2. **`payment` and `payments` are different terms.** There is no stemming, so they scored
   separately (1 + 3). With stemming they'd merge, and the total would *change*. That is
   exactly why the README insists a tokenizer change starts with a new evaluation case.

The test itself only asserts `heuristicScore > 0`
([`OperationsServiceTests.cs:94`](../tests/UnitTests/OperationsServiceTests.cs#L94)), so the
17 is something you derived, not something a test pins. Exercise 3 below fixes that.

## What the tokenizer cannot do

No stemming, no synonyms, no semantic equivalence, ASCII-only (`[a-z0-9]`: an accented
or non-Latin word is split or dropped entirely). A useful next experiment would add a fixed
evaluation case demonstrating the desired behavior *before* changing tokenization.
Increasing complexity without an evaluation expectation makes ranking changes impossible to
review: you can't tell an improvement from a regression.

## Exercises

**1 (easy).** *Goal:* apply the threshold. Score the query `queue` against a document with
`queue` only in content. *Check:* score 1 < 3, so it is excluded. That's the same reason
`Billing` is absent from the `queue` results at
[`LexicalRetrieverTests.cs:37-39`](../tests/UnitTests/LexicalRetrieverTests.cs#L37-L39)
(Billing contains no `queue` at all).

**2 (medium).** *Goal:* predict the eval file's outcome. For each case in
[`sample-eval-cases/retrieval.json`](../sample-eval-cases/retrieval.json), score both
fixture documents (`Billing`, `Auth` at [`LexicalRetrieverTests.cs:9-19`](../tests/UnitTests/LexicalRetrieverTests.cs#L9-L19)).
*Check:* `BillingService timeout checkout` → Billing scores 4 (`billingservice` in title)
+ 7 (`timeout` title + tags) + 1 (`checkout` in content) = 12; Auth scores 0. `AuthService
signing key` → Auth scores 4 + 1 + 1 = 6; Billing 0. `printer toner tray` → nothing
passes. That matches the expectations the test enforces at lines 64-73.

**3 (hard).** *Goal:* turn the hand calculation into a pinned behavior. Write the assertion
you would add to `Investigation_persists_truthful_serialized_evidence_and_pending_approval`
so that a future tokenizer change which merges `payment`/`payments` *fails loudly*.
*Check:* an assertion on `heuristicScore == 17` plus `matchedTerms` equal to
`["billing","payment","capture","payments"]` (the JSON property is `MatchedTerms`;
`System.Text.Json` keeps the C# name for that member because the anonymous object at line
368 doesn't rename it). Then ask yourself whether pinning 17 is *wise*: it makes legitimate
ranking improvements require a test edit. That trade-off (characterization vs.
brittleness) is a mid-level review conversation, and there's no single right answer.

## Source excerpt

From [src/Application/Retrieval/LexicalRetriever.cs](../src/Application/Retrieval/LexicalRetriever.cs).

```cs
    private static RetrievalMatch Score(RetrievalDocument document, string[] queryTerms)
    {
        var title = Tokens(document.Title).ToHashSet(StringComparer.Ordinal);
        var tags = Tokens(document.Tags).ToHashSet(StringComparer.Ordinal);
        var content = Tokens(document.Content).ToHashSet(StringComparer.Ordinal);
        var matched = new List<string>();
        var score = 0;
        foreach (var term in queryTerms)
        {
            var termScore = (title.Contains(term) ? 4 : 0)
                + (tags.Contains(term) ? 3 : 0)
                + (content.Contains(term) ? 1 : 0);
            if (termScore == 0) continue;
            matched.Add(term);
            score += termScore;
        }
        return new RetrievalMatch(document.Id, document.Title, score, matched.ToArray());
    }

    private static IEnumerable<string> Tokens(string value) =>
        TokenPattern().Matches(value.ToLowerInvariant()).Select(match => match.Value);

    [GeneratedRegex("[a-z0-9]+", RegexOptions.CultureInvariant)]
    private static partial Regex TokenPattern();
}
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)
