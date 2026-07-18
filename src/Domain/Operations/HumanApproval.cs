namespace NetAiOps.Domain.Operations;

public sealed class HumanApproval
{
    public Guid Id { get; set; }
    public required string RecommendationType { get; set; }
    public required string RecommendationJson { get; set; }
    public required string Status { get; set; }
    public Guid RequestedByUserId { get; set; }
    public Guid? DecidedByUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? DecidedAt { get; set; }
}
