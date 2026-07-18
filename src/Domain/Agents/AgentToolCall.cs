namespace NetAiOps.Domain.Agents;

public sealed class AgentToolCall
{
    public Guid Id { get; set; }
    public Guid AgentRunId { get; set; }
    public required string ToolName { get; set; }
    public required string InputJson { get; set; }
    public required string OutputJson { get; set; }
    public required string Status { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
}
