using System.ComponentModel.DataAnnotations;

namespace NetAiOps.Application.Operations;

public sealed record TicketListItemDto(
    Guid Id,
    string Title,
    string Status,
    string Priority,
    string Category,
    Guid CreatedByUserId,
    Guid? AssignedToUserId,
    Guid? RelatedIncidentId,
    string[] Tags,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record TicketDetailDto(
    Guid Id,
    string Title,
    string Description,
    string Status,
    string Priority,
    string Category,
    Guid CreatedByUserId,
    Guid? AssignedToUserId,
    Guid? RelatedIncidentId,
    string[] Tags,
    string? AiSummary,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? ResolvedAt,
    IReadOnlyList<TicketCommentDto> Comments,
    IReadOnlyList<AuditEventDto> AuditEvents);

public sealed record TicketCommentDto(Guid Id, Guid AuthorUserId, string Body, bool IsInternal, DateTimeOffset CreatedAt);
public sealed record AuditEventDto(Guid Id, string EventType, string Summary, DateTimeOffset CreatedAt);

public sealed record CreateTicketRequest(
    [property: Required, MinLength(5), MaxLength(160)] string Title,
    [property: Required, MinLength(10), MaxLength(4000)] string Description,
    string Priority,
    string Category,
    Guid? AssignedToUserId,
    string[] Tags);

public sealed record UpdateTicketStatusRequest([property: Required] string Status);
public sealed record AssignTicketRequest(Guid? AssignedToUserId);
public sealed record AddTicketCommentRequest([property: Required, MinLength(2)] string Body, bool IsInternal);

public sealed record IncidentListItemDto(
    Guid Id,
    string Title,
    string Severity,
    string Status,
    string PrimaryService,
    Guid? CommanderUserId,
    DateTimeOffset DetectedAt,
    DateTimeOffset UpdatedAt);

public sealed record IncidentDetailDto(
    Guid Id,
    string Title,
    string Description,
    string Severity,
    string Status,
    DateTimeOffset StartedAt,
    DateTimeOffset DetectedAt,
    DateTimeOffset? ResolvedAt,
    string PrimaryService,
    Guid CreatedByUserId,
    Guid? CommanderUserId,
    string? RootCauseSummary,
    string? CustomerImpact,
    string? ResolutionSummary,
    string[] AffectedServices,
    string? AiRootCauseCandidate,
    string? AiRootCauseStatus,
    IReadOnlyList<IncidentTimelineEventDto> TimelineEvents);

public sealed record IncidentTimelineEventDto(
    Guid Id,
    Guid ActorUserId,
    string EventType,
    string Summary,
    bool IsAiGenerated,
    DateTimeOffset OccurredAt);

public sealed record CreateIncidentRequest(
    [property: Required, MinLength(5), MaxLength(180)] string Title,
    [property: Required, MinLength(10), MaxLength(4000)] string Description,
    string Severity,
    string PrimaryService,
    string[] AffectedServices);

public sealed record AddIncidentTimelineRequest([property: Required] string EventType, [property: Required] string Summary);

public sealed record LogEntryDto(
    Guid Id,
    DateTimeOffset Timestamp,
    string ServiceName,
    string Environment,
    string Level,
    string Message,
    string TraceId,
    string CorrelationId,
    string Host,
    string? ExceptionType,
    Guid? LinkedIncidentId);

public sealed record KnowledgeDocumentDto(Guid Id, string Title, string SourceType, string[] Tags, DateTimeOffset UpdatedAt);
public sealed record AgentRunDto(Guid Id, string AgentName, string Status, Guid? RelatedTicketId, Guid? RelatedIncidentId, int TotalToolCalls, DateTimeOffset StartedAt);
public sealed record EvalRunDto(Guid Id, string SuiteName, string Status, int TotalTests, int PassedTests, int FailedTests, double AverageScore, DateTimeOffset StartedAt);
public sealed record AdminSettingDto(Guid Id, string Key, string Value, string Description, DateTimeOffset UpdatedAt);
public sealed record HumanApprovalDto(Guid Id, string RecommendationType, string Status, DateTimeOffset CreatedAt);
