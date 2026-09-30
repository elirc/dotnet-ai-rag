using NetAiOps.Application.Retrieval;

var checks = 0;
void Check(bool condition, string property)
{
    checks++;
    if (!condition) throw new Exception(property);
}
Guid Id(int n) => Guid.Parse($"00000000-0000-0000-0000-{n:D12}");
var a = new RetrievalDocument(Id(1), "Queue Recovery", "operations", "inspect queue depth");
var b = new RetrievalDocument(Id(2), "Worker Guide", "queue", "worker guide");
var c = new RetrievalDocument(Id(3), "Generic Guide", "general", "queue");
var documents = new[] { a, b, c };
var matches = LexicalRetriever.Search(documents, "queue");
Check(matches.Select(x => x.DocumentId).SequenceEqual(new[] { a.Id, b.Id }), "threshold and order");
Check(matches.Select(x => x.HeuristicScore).SequenceEqual(new[] { 5, 3 }), "independent field arithmetic");
Check(LexicalRetriever.Search(documents, "queue queue").Select(x => x.HeuristicScore).SequenceEqual(new[] { 5, 3 }), "query repetition");
Check(LexicalRetriever.Search(documents, "incident service investigate").Count == 0, "stop-word-only");
Check(LexicalRetriever.Search(documents, "queue", topK: 0).Count == 0, "zero topK");
Check(LexicalRetriever.Search(documents, "queue", minimumScore: 0).Count == 0, "zero threshold");
Check(LexicalRetriever.Search(documents, "queue", topK: 1).Single().DocumentId == a.Id, "topK bound");
Check(LexicalRetriever.Search(documents, "queue-depth").First().HeuristicScore == 6, "punctuation creates two terms");
Check(LexicalRetriever.Search(documents, "depth queue").First().MatchedTerms.SequenceEqual(new[] { "depth", "queue" }), "matched-term query order");
var repeated = new[] { a with { Content = "queue queue depth queue" }, b, c };
Check(LexicalRetriever.Search(repeated, "queue").First().HeuristicScore == 5, "field occurrence repetition");
var first = new RetrievalDocument(Id(10), "Same", "queue", "");
var later = new RetrievalDocument(Id(11), "same", "queue", "");
foreach (var order in new[] { new[] { first, later }, new[] { later, first } })
    Check(LexicalRetriever.Search(order, "queue").Select(x => x.DocumentId).SequenceEqual(new[] { first.Id, later.Id }), "actual ordinal-ignore-case then Guid order");
foreach (var order in new[] { new[] { a,b,c }, new[] { a,c,b }, new[] { b,a,c }, new[] { b,c,a }, new[] { c,a,b }, new[] { c,b,a } })
    Check(LexicalRetriever.Search(order, "queue").Select(x => x.DocumentId).SequenceEqual(new[] { a.Id,b.Id }), "all fixture permutations");
Console.WriteLine($"{checks} actual-source retrieval assertions passed; no API, EF provider, or model call executed.");
