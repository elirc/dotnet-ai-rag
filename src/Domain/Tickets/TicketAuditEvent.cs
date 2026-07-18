namespace NetAiOps.Domain.Tickets;

public sealed class TicketAuditEvent
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid ActorUserId { get; set; }
    public required string EventType { get; set; }
    public required string Summary { get; set; }
    public string? MetadataJson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
