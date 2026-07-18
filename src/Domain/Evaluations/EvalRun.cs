namespace NetAiOps.Domain.Evaluations;

public sealed class EvalRun
{
    public Guid Id { get; set; }
    public required string SuiteName { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public required string Status { get; set; }
    public int TotalTests { get; set; }
    public int PassedTests { get; set; }
    public int FailedTests { get; set; }
    public double AverageScore { get; set; }
    public string? Notes { get; set; }
}
