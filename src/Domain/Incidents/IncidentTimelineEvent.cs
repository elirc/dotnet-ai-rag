namespace NetAiOps.Domain.Incidents;

public sealed class IncidentTimelineEvent
{
    public Guid Id { get; set; }
    public Guid IncidentId { get; set; }
    public Guid ActorUserId { get; set; }
    public required string EventType { get; set; }
    public required string Summary { get; set; }
    public bool IsAiGenerated { get; set; }
    public DateTimeOffset OccurredAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
