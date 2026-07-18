namespace NetAiOps.Domain.Tickets;

public sealed class Ticket
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public TicketCategory Category { get; set; } = TicketCategory.Unknown;
    public Guid CreatedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public Guid? RelatedIncidentId { get; set; }
    public required string TagsCsv { get; set; }
    public string? AiSummary { get; set; }
    public string? AiSuggestedPriority { get; set; }
    public string? AiSuggestedCategory { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? ResolvedAt { get; set; }
    public uint Version { get; set; }

    public List<TicketComment> Comments { get; set; } = [];
    public List<TicketAuditEvent> AuditEvents { get; set; } = [];
}
