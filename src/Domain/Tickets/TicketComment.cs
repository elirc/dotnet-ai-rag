namespace NetAiOps.Domain.Tickets;

public sealed class TicketComment
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid AuthorUserId { get; set; }
    public required string Body { get; set; }
    public bool IsInternal { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
