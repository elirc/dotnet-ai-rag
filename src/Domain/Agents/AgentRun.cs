namespace NetAiOps.Domain.Agents;

public sealed class AgentRun
{
    public Guid Id { get; set; }
    public required string AgentName { get; set; }
    public required string InputJson { get; set; }
    public required string OutputJson { get; set; }
    public required string Status { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Guid? RelatedTicketId { get; set; }
    public Guid? RelatedIncidentId { get; set; }
    public int TotalToolCalls { get; set; }
    public int TotalEstimatedTokens { get; set; }
    public decimal EstimatedCost { get; set; }

    public List<AgentToolCall> ToolCalls { get; set; } = [];
}
