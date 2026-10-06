# Netairag: evidence before recommendations

This course follows the incident investigation command from authorization to persisted evidence. Previously the command counted log rows and constructed a recommendation without retrieving the knowledge documents already stored by the application. Its recorded confidence, token estimates, tool count, and timestamps could imply work that never happened. The improvement makes the implemented behavior explicit: lexical retrieval, bounded log evidence, two recorded tools, and a human approval request.

Start with the codebase map, then calculate the scoring example in Concepts. Read the worked change before running tests. The practice chapter asks you to predict behavior for missing evidence and unauthorized callers; compare your reasoning with the supplied solutions before attempting a variation. The trace lab ties those predictions to the saved JSON and approval record.

The central distinction is between a document matching an incident description and a document proving its cause. Search finds candidates. The system still asks a person to compare runbook instructions with incident evidence. A lexical score is therefore named heuristicScore, modelConfidence is null, and modelUsed is false. No model service is contacted by this path.

The tests exercise ranking directly and application persistence through an isolated EF InMemory fixture. That fixture can demonstrate the service's saved relationships and serialized fields; it cannot establish PostgreSQL transaction isolation or production query performance. Read the final verification evidence with that boundary in mind. Existing project guides remain useful for setup and architecture, while this course concentrates on one observable investigation workflow.

## Route and time budget

| Chapter | You'll be able to… | Time |
| --- | --- | --- |
| [01 Codebase map](01-CODEBASE-MAP.md) | name every hop from `POST …/run-investigation` to the single `SaveChangesAsync`, with line numbers | 45 min |
| [02 Concepts](02-CONCEPTS.md) | score any query against any document by hand (worked example: the test fixture scores 17) | 1 h |
| [03 Worked change](03-WORKED-CHANGE.md) | explain the before/after diff and the five values the old code invented, and list what the change left unfixed | 1 h |
| [04 Testing & debugging](04-TESTING-AND-DEBUGGING.md) | say what each of the 11 test cases pins, and what none of them pins | 1 h |
| [05 Practice](05-PRACTICE.md) → [06 Solutions](06-SOLUTIONS-AND-REVIEW.md) | seven exercises, easy → hard, each with a Check you can confirm against code | 2-3 h |
| [07 Trace lab](07-TRACE-LAB.md) | trace the test fixture *and* the seeded demo data into saved rows | 1 h |
| [VERIFICATION](VERIFICATION.md) | read the recorded evidence (and its gaps) like a reviewer | 15 min |

Then continue with the [extended course](extended-course/README.md): 32 deeper lessons
(approval API boundary, outbox semantics, concurrency, corpus scale, document trust) with
separate answer files.

**Before you start:** the repo also contains two older documentation trees,
[`docs/upskill/`](../docs/upskill/README.md) and [`docs2.0/`](../docs2.0/README.md). They
predate the retrieval change described here, so where they describe the investigation
command or quote test counts, this course is the current one.

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
