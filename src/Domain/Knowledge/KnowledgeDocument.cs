namespace NetAiOps.Domain.Knowledge;

public sealed class KnowledgeDocument
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string SourceType { get; set; }
    public required string Content { get; set; }
    public string? FileName { get; set; }
    public required string TagsCsv { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
