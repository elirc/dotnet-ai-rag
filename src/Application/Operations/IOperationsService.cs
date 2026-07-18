using NetAiOps.Application.Common;

namespace NetAiOps.Application.Operations;

public interface IOperationsService
{
    Task<PagedResult<TicketListItemDto>> ListTicketsAsync(RequestContext context, string? status, string? priority, int page, int pageSize, CancellationToken ct);
    Task<TicketDetailDto?> GetTicketAsync(RequestContext context, Guid id, CancellationToken ct);
    Task<TicketDetailDto> CreateTicketAsync(RequestContext context, CreateTicketRequest request, CancellationToken ct);
    Task<TicketDetailDto?> UpdateTicketStatusAsync(RequestContext context, Guid id, UpdateTicketStatusRequest request, CancellationToken ct);
    Task<TicketDetailDto?> AssignTicketAsync(RequestContext context, Guid id, AssignTicketRequest request, CancellationToken ct);
    Task<TicketDetailDto?> AddTicketCommentAsync(RequestContext context, Guid id, AddTicketCommentRequest request, CancellationToken ct);

    Task<PagedResult<IncidentListItemDto>> ListIncidentsAsync(RequestContext context, string? status, string? severity, int page, int pageSize, CancellationToken ct);
    Task<IncidentDetailDto?> GetIncidentAsync(RequestContext context, Guid id, CancellationToken ct);
    Task<IncidentDetailDto> CreateIncidentAsync(RequestContext context, CreateIncidentRequest request, CancellationToken ct);
    Task<IncidentDetailDto?> AddIncidentTimelineAsync(RequestContext context, Guid id, AddIncidentTimelineRequest request, CancellationToken ct);
    Task<AgentRunDto?> RunIncidentInvestigationAsync(RequestContext context, Guid id, CancellationToken ct);

    Task<PagedResult<LogEntryDto>> SearchLogsAsync(RequestContext context, string? serviceName, string? level, string? query, int page, int pageSize, CancellationToken ct);
    Task<IReadOnlyList<KnowledgeDocumentDto>> ListKnowledgeDocumentsAsync(RequestContext context, CancellationToken ct);
    Task<IReadOnlyList<AgentRunDto>> ListAgentRunsAsync(RequestContext context, CancellationToken ct);
    Task<IReadOnlyList<EvalRunDto>> ListEvalRunsAsync(RequestContext context, CancellationToken ct);
    Task<IReadOnlyList<AdminSettingDto>> ListAdminSettingsAsync(RequestContext context, CancellationToken ct);
    Task<IReadOnlyList<HumanApprovalDto>> ListApprovalsAsync(RequestContext context, CancellationToken ct);
}
