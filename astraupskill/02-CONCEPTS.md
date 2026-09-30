# Deterministic ranking and truthful measurements

Lexical retrieval answers a narrow question: which stored documents contain the query's meaningful tokens? Tokens are lowercased ASCII letter-or-digit runs. A small stop-word set removes common investigation vocabulary. Distinct query terms prevent repeating a word in an incident description from multiplying its contribution. Each document field is also represented as a set, so repeating a keyword within one field does not increase its score.

A term found in the title contributes four points, in tags three, and in content one. The contributions are additive across fields. A document with queue in its title and content scores five for that query; one with queue only in tags scores three. Both pass the default minimum of three. A content-only occurrence scores one and is excluded. These weights are an explicit heuristic, not a calibrated probability.

Results sort by descending score, then case-insensitive ordinal title, then identifier. The final identifier tie-break prevents equal-title documents from changing places with input enumeration order. Take applies the requested bound after filtering and ordering. Nonpositive bounds or thresholds return no results.

The current tokenizer has a clear limitation: it does not provide stemming, semantic equivalence, or general multilingual segmentation. A useful next experiment would add a fixed evaluation case demonstrating the desired behavior before changing tokenization. Increasing complexity without an evaluation expectation makes ranking changes difficult to review. The investigation output separately records that no model was used, rather than dressing these integer scores as model confidence.

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
