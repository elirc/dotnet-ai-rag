namespace NetAiOps.Domain.Operations;

public sealed class AuditEvent
{
    public Guid Id { get; set; }
    public required string EntityType { get; set; }
    public Guid EntityId { get; set; }
    public Guid ActorUserId { get; set; }
    public required string EventType { get; set; }
    public required string Summary { get; set; }
    public string? MetadataJson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
