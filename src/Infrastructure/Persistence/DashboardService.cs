using NetAiOps.Application.Dashboard;
using Microsoft.EntityFrameworkCore;
using NetAiOps.Domain.Incidents;
using NetAiOps.Domain.Tickets;

namespace NetAiOps.Infrastructure.Persistence;

public sealed class DashboardService(NetAiOpsDbContext db) : IDashboardService
{
    public async Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        var openTickets = await db.Tickets.CountAsync(x => x.Status != TicketStatus.Closed && x.Status != TicketStatus.Resolved, ct);
        var activeIncidents = await db.Incidents.CountAsync(x => x.Status != IncidentStatus.Closed && x.Status != IncidentStatus.Resolved, ct);
        var criticalAlerts = await db.Logs.CountAsync(x => x.Level == "Critical" && x.Timestamp >= now.AddHours(-24), ct);
        var resolvedTickets = await db.Tickets.Where(x => x.ResolvedAt != null).ToListAsync(ct);
        var averageResolutionHours = resolvedTickets.Count == 0
            ? 0
            : Math.Round(resolvedTickets.Average(x => (x.ResolvedAt!.Value - x.CreatedAt).TotalHours), 1);
        var agentRunsToday = await db.AgentRuns.CountAsync(x => x.StartedAt >= now.Date, ct);
        var failedBackgroundJobs = await db.OutboxMessages.CountAsync(x => x.Status == "Failed", ct);
        var latestEval = await db.EvalRuns.OrderByDescending(x => x.StartedAt).FirstOrDefaultAsync(ct);
        var approvals = await db.HumanApprovals
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .Select(x => new AiRecommendationDto(
                x.Id.ToString(),
                x.RecommendationType,
                "Human approval queue",
                x.Status,
                x.CreatedAt))
            .ToListAsync(ct);

        if (approvals.Count == 0)
        {
            approvals.Add(new AiRecommendationDto(
                "seed-rec-001",
                "No pending approvals; run an incident investigation agent to populate this queue.",
                "System",
                "Info",
                now));
        }

        return new DashboardSummaryDto(
            openTickets,
            activeIncidents,
            criticalAlerts,
            averageResolutionHours,
            agentRunsToday,
            failedBackgroundJobs,
            approvals,
            latestEval?.AverageScore ?? 0);
    }
}
