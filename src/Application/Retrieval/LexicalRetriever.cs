using System.Text.RegularExpressions;

namespace NetAiOps.Application.Retrieval;

public sealed record RetrievalDocument(Guid Id, string Title, string Tags, string Content);
public sealed record RetrievalMatch(Guid DocumentId, string Title, int HeuristicScore, string[] MatchedTerms);

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
