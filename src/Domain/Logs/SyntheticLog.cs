namespace NetAiOps.Domain.Logs;

public sealed class SyntheticLog
{
    public Guid Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public required string ServiceName { get; set; }
    public required string Environment { get; set; }
    public required string Level { get; set; }
    public required string Message { get; set; }
    public required string TraceId { get; set; }
    public required string CorrelationId { get; set; }
    public required string Host { get; set; }
    public string? ExceptionType { get; set; }
    public string? StackTrace { get; set; }
    public string? MetadataJson { get; set; }
    public Guid? LinkedIncidentId { get; set; }
}
