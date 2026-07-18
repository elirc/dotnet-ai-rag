namespace NetAiOps.Application.Dashboard;

public sealed record DashboardSummaryDto(
    int OpenTickets,
    int ActiveIncidents,
    int CriticalAlerts,
    double AverageResolutionHours,
    int AgentInvestigationsToday,
    int FailedBackgroundJobs,
    IReadOnlyList<AiRecommendationDto> RecentAiRecommendations,
    double EvalPassRate);

public sealed record AiRecommendationDto(
    string Id,
    string Title,
    string Source,
    string Status,
    DateTimeOffset CreatedAt);
