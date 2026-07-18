namespace NetAiOps.Domain.Incidents;

public sealed class Incident
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public IncidentSeverity Severity { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.Detected;
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset DetectedAt { get; set; }
    public DateTimeOffset? ResolvedAt { get; set; }
    public required string PrimaryService { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Guid? CommanderUserId { get; set; }
    public string? RootCauseSummary { get; set; }
    public string? CustomerImpact { get; set; }
    public string? ResolutionSummary { get; set; }
    public string? AffectedServicesCsv { get; set; }
    public string? AiRootCauseCandidate { get; set; }
    public string? AiRootCauseStatus { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public uint Version { get; set; }

    public List<IncidentTimelineEvent> TimelineEvents { get; set; } = [];
}
