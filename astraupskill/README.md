# Netairag: evidence before recommendations

This course follows the incident investigation command from authorization to persisted evidence. Previously the command counted log rows and constructed a recommendation without retrieving the knowledge documents already stored by the application. Its recorded confidence, token estimates, tool count, and timestamps could imply work that never happened. The improvement makes the implemented behavior explicit: lexical retrieval, bounded log evidence, two recorded tools, and a human approval request.

Start with the codebase map, then calculate the scoring example in Concepts. Read the worked change before running tests. The practice chapter asks you to predict behavior for missing evidence and unauthorized callers; compare your reasoning with the supplied solutions before attempting a variation. The trace lab ties those predictions to the saved JSON and approval record.

The central distinction is between a document matching an incident description and a document proving its cause. Search finds candidates. The system still asks a person to compare runbook instructions with incident evidence. A lexical score is therefore named heuristicScore, modelConfidence is null, and modelUsed is false. No model service is contacted by this path.

The tests exercise ranking directly and application persistence through an isolated EF InMemory fixture. That fixture can demonstrate the service's saved relationships and serialized fields; it cannot establish PostgreSQL transaction isolation or production query performance. Read the final verification evidence with that boundary in mind. Existing project guides remain useful for setup and architecture, while this course concentrates on one observable investigation workflow.

## Source excerpt

From [src/Application/Retrieval/LexicalRetriever.cs](../src/Application/Retrieval/LexicalRetriever.cs).

```cs
public static partial class LexicalRetriever
{
    private static readonly HashSet<string> StopWords = new(StringComparer.Ordinal)
    {
        "a", "an", "and", "for", "in", "of", "on", "or", "the", "to", "with",
        "incident", "investigate", "service"
    };

    public static IReadOnlyList<RetrievalMatch> Search(
        IEnumerable<RetrievalDocument> documents,
        string query,
        int topK = 3,
        int minimumScore = 3)
    {
        if (topK <= 0 || minimumScore <= 0) return [];
        var queryTerms = Tokens(query).Where(x => !StopWords.Contains(x)).Distinct().ToArray();
        if (queryTerms.Length == 0) return [];

        return documents.Select(document => Score(document, queryTerms))
            .Where(match => match.HeuristicScore >= minimumScore)
            .OrderByDescending(match => match.HeuristicScore)
            .ThenBy(match => match.Title, StringComparer.OrdinalIgnoreCase)
            .ThenBy(match => match.DocumentId)
            .Take(topK)
            .ToArray();
    }

    private static RetrievalMatch Score(RetrievalDocument document, string[] queryTerms)
    {
        var title = Tokens(document.Title).ToHashSet(StringComparer.Ordinal);
        var tags = Tokens(document.Tags).ToHashSet(StringComparer.Ordinal);
        var content = Tokens(document.Content).ToHashSet(StringComparer.Ordinal);
```

## Course navigation

[README](README.md) / [01-CODEBASE-MAP](01-CODEBASE-MAP.md) / [02-CONCEPTS](02-CONCEPTS.md) / [03-WORKED-CHANGE](03-WORKED-CHANGE.md) / [04-TESTING-AND-DEBUGGING](04-TESTING-AND-DEBUGGING.md) / [05-PRACTICE](05-PRACTICE.md) / [06-SOLUTIONS-AND-REVIEW](06-SOLUTIONS-AND-REVIEW.md) / [07-TRACE-LAB](07-TRACE-LAB.md) / [VERIFICATION](VERIFICATION.md)


## Extended learning route

[Open the deeper lessons, isolated labs, answers and capstone](extended-course/README.md).
