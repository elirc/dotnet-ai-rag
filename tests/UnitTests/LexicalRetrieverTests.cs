using FluentAssertions;
using NetAiOps.Application.Retrieval;
using System.Text.Json;

namespace NetAiOps.UnitTests;

public sealed class LexicalRetrieverTests
{
    private static readonly RetrievalDocument Billing = new(
        Guid.Parse("00000000-0000-0000-0000-000000000001"),
        "BillingService Timeout Runbook",
        "billing,runbook,timeout",
        "Inspect database latency, connection pool saturation, retries, and slow checkout queries.");

    private static readonly RetrievalDocument Auth = new(
        Guid.Parse("00000000-0000-0000-0000-000000000002"),
        "AuthService Runbook",
        "auth,runbook,jwt",
        "Token validation failures may indicate a signing key mismatch or stale configuration.");

    [Fact]
    public void Billing_and_auth_queries_rank_the_matching_runbook_first()
    {
        LexicalRetriever.Search([Auth, Billing], "BillingService timeout checkout")[0]
            .DocumentId.Should().Be(Billing.Id);
        LexicalRetriever.Search([Billing, Auth], "AuthService signing key")[0]
            .DocumentId.Should().Be(Auth.Id);
    }

    [Fact]
    public void Ranking_is_stable_bounded_and_uses_title_then_id_for_ties()
    {
        var b = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000011"), "B Runbook", "queue", "queue");
        var a = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000012"), "A Runbook", "queue", "queue");
        var laterId = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000014"), "Same Runbook", "cache", "cache");
        var earlierId = new RetrievalDocument(Guid.Parse("00000000-0000-0000-0000-000000000013"), "Same Runbook", "cache", "cache");
        var results = LexicalRetriever.Search([b, a, Billing], "queue", topK: 1);
        results.Should().ContainSingle();
        results[0].Title.Should().Be("A Runbook");
        LexicalRetriever.Search([laterId, earlierId], "cache")
            .Select(x => x.DocumentId).Should().Equal(earlierId.Id, laterId.Id);
    }

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
                matches.Should().NotBeEmpty(evaluation.Id);
                matches[0].Title.Should().Be(evaluation.ExpectedTopTitle, evaluation.Id);
            }
        }
    }

    private sealed record EvaluationCase(
        string Id,
        string Query,
        string? ExpectedTopTitle,
        int? ExpectedMatches);
}
