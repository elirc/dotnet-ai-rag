using Microsoft.EntityFrameworkCore;
using NetAiOps.Application.Common;
using NetAiOps.Application.Operations;
using NetAiOps.Domain.Agents;
using NetAiOps.Domain.Incidents;
using NetAiOps.Domain.Operations;
using NetAiOps.Domain.Tickets;
using NetAiOps.Infrastructure.Persistence;

namespace NetAiOps.Infrastructure.Operations;

public sealed class OperationsService(NetAiOpsDbContext db) : IOperationsService
{
    private const int MaxPageSize = 100;

    public async Task<PagedResult<TicketListItemDto>> ListTicketsAsync(RequestContext context, string? status, string? priority, int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var query = db.Tickets.AsNoTracking();
        if (!context.IsAdminOrEngineer)
        {
            query = query.Where(x => x.CreatedByUserId == context.UserId || x.AssignedToUserId == context.UserId);
        }

        if (TryParse<TicketStatus>(status, out var ticketStatus))
        {
            query = query.Where(x => x.Status == ticketStatus);
        }

        if (TryParse<TicketPriority>(priority, out var ticketPriority))
        {
            query = query.Where(x => x.Priority == ticketPriority);
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(x => x.Priority)
            .ThenByDescending(x => x.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new TicketListItemDto(
                x.Id,
                x.Title,
                x.Status.ToString(),
                x.Priority.ToString(),
                x.Category.ToString(),
                x.CreatedByUserId,
                x.AssignedToUserId,
                x.RelatedIncidentId,
                SplitTags(x.TagsCsv),
                x.CreatedAt,
                x.UpdatedAt))
            .ToListAsync(ct);

        return new PagedResult<TicketListItemDto>(items, page, pageSize, total);
    }

    public async Task<TicketDetailDto?> GetTicketAsync(RequestContext context, Guid id, CancellationToken ct)
    {
        var ticket = await db.Tickets
            .AsNoTracking()
            .Include(x => x.Comments)
            .Include(x => x.AuditEvents)
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        if (ticket is null || !CanReadTicket(context, ticket))
        {
            return null;
        }

        return ToTicketDetail(ticket);
    }

    public async Task<TicketDetailDto> CreateTicketAsync(RequestContext context, CreateTicketRequest request, CancellationToken ct)
    {
        EnsureSupportMutation(context);
        var now = DateTimeOffset.UtcNow;
        var priority = ParseOrDefault(request.Priority, TicketPriority.Medium);
        var category = ParseOrDefault(request.Category, TicketCategory.Unknown);

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Status = TicketStatus.New,
            Priority = priority,
            Category = category,
            CreatedByUserId = context.UserId,
            AssignedToUserId = request.AssignedToUserId,
            TagsCsv = JoinTags(request.Tags),
            AiSummary = $"Mock triage: {category} ticket with {priority} priority. Review service logs and related incidents before escalation.",
            AiSuggestedPriority = priority.ToString(),
            AiSuggestedCategory = category.ToString(),
            CreatedAt = now,
            UpdatedAt = now
        };

        ticket.Comments.Add(new TicketComment
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            AuthorUserId = context.UserId,
            Body = "Ticket created.",
            IsInternal = true,
            CreatedAt = now
        });

        AddTicketAudit(ticket, context.UserId, "TicketCreated", "Ticket created", now);
        db.Tickets.Add(ticket);
        AddGlobalAudit("Ticket", ticket.Id, context.UserId, "TicketCreated", $"Created ticket {ticket.Title}", now);
        AddOutbox("TicketCreated", $$"""{"ticketId":"{{ticket.Id}}","priority":"{{ticket.Priority}}"}""", now);
        await db.SaveChangesAsync(ct);

        return ToTicketDetail(ticket);
    }

    public async Task<TicketDetailDto?> UpdateTicketStatusAsync(RequestContext context, Guid id, UpdateTicketStatusRequest request, CancellationToken ct)
    {
        EnsureSupportMutation(context);
        var ticket = await db.Tickets.Include(x => x.Comments).Include(x => x.AuditEvents).SingleOrDefaultAsync(x => x.Id == id, ct);
        if (ticket is null || !CanReadTicket(context, ticket))
        {
            return null;
        }

        var next = ParseOrDefault(request.Status, ticket.Status);
        var previous = ticket.Status;
        if (!IsAllowedTicketTransition(previous, next))
        {
            throw new InvalidOperationException($"Ticket cannot move from {previous} to {next}.");
        }

        var now = DateTimeOffset.UtcNow;
        ticket.Status = next;
        ticket.UpdatedAt = now;
        ticket.ResolvedAt = next is TicketStatus.Resolved or TicketStatus.Closed ? now : ticket.ResolvedAt;
        ticket.Version++;
        AddTicketAudit(ticket, context.UserId, "StatusChanged", $"Status changed from {previous} to {next}", now);
        AddGlobalAudit("Ticket", ticket.Id, context.UserId, "StatusChanged", $"Ticket status changed from {previous} to {next}", now);
        await db.SaveChangesAsync(ct);
        return ToTicketDetail(ticket);
    }

    public async Task<TicketDetailDto?> AssignTicketAsync(RequestContext context, Guid id, AssignTicketRequest request, CancellationToken ct)
    {
        EnsureSupportMutation(context);
        var ticket = await db.Tickets.Include(x => x.Comments).Include(x => x.AuditEvents).SingleOrDefaultAsync(x => x.Id == id, ct);
        if (ticket is null || !CanReadTicket(context, ticket))
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;
        ticket.AssignedToUserId = request.AssignedToUserId;
        ticket.UpdatedAt = now;
        ticket.Version++;
        AddTicketAudit(ticket, context.UserId, "Assigned", request.AssignedToUserId is null ? "Ticket unassigned" : $"Ticket assigned to {request.AssignedToUserId}", now);
        AddGlobalAudit("Ticket", ticket.Id, context.UserId, "Assigned", "Ticket assignment changed", now);
        AddOutbox("TicketAssignmentChanged", $$"""{"ticketId":"{{ticket.Id}}","assignedTo":"{{request.AssignedToUserId}}"}""", now);
        await db.SaveChangesAsync(ct);
        return ToTicketDetail(ticket);
    }

    public async Task<TicketDetailDto?> AddTicketCommentAsync(RequestContext context, Guid id, AddTicketCommentRequest request, CancellationToken ct)
    {
        EnsureSupportMutation(context);
        var ticket = await db.Tickets.Include(x => x.Comments).Include(x => x.AuditEvents).SingleOrDefaultAsync(x => x.Id == id, ct);
        if (ticket is null || !CanReadTicket(context, ticket))
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;
        ticket.Comments.Add(new TicketComment
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            AuthorUserId = context.UserId,
            Body = request.Body.Trim(),
            IsInternal = request.IsInternal,
            CreatedAt = now
        });
        ticket.UpdatedAt = now;
        ticket.Version++;
        AddTicketAudit(ticket, context.UserId, request.IsInternal ? "InternalNoteAdded" : "CommentAdded", request.IsInternal ? "Internal note added" : "Comment added", now);
        AddGlobalAudit("Ticket", ticket.Id, context.UserId, request.IsInternal ? "InternalNoteAdded" : "CommentAdded", "Ticket discussion updated", now);
        await db.SaveChangesAsync(ct);
        return ToTicketDetail(ticket);
    }

    public async Task<PagedResult<IncidentListItemDto>> ListIncidentsAsync(RequestContext context, string? status, string? severity, int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var query = db.Incidents.AsNoTracking();
        if (TryParse<IncidentStatus>(status, out var incidentStatus))
        {
            query = query.Where(x => x.Status == incidentStatus);
        }

        if (TryParse<IncidentSeverity>(severity, out var incidentSeverity))
        {
            query = query.Where(x => x.Severity == incidentSeverity);
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(x => x.Severity)
            .ThenByDescending(x => x.DetectedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new IncidentListItemDto(x.Id, x.Title, x.Severity.ToString(), x.Status.ToString(), x.PrimaryService, x.CommanderUserId, x.DetectedAt, x.UpdatedAt))
            .ToListAsync(ct);

        return new PagedResult<IncidentListItemDto>(items, page, pageSize, total);
    }

    public async Task<IncidentDetailDto?> GetIncidentAsync(RequestContext context, Guid id, CancellationToken ct)
    {
        var incident = await db.Incidents.AsNoTracking().Include(x => x.TimelineEvents).SingleOrDefaultAsync(x => x.Id == id, ct);
        return incident is null ? null : ToIncidentDetail(incident);
    }

    public async Task<IncidentDetailDto> CreateIncidentAsync(RequestContext context, CreateIncidentRequest request, CancellationToken ct)
    {
        if (!context.IsAdminOrEngineer)
        {
            throw new UnauthorizedAccessException("Only Admin and Engineer users can create incidents.");
        }

        var now = DateTimeOffset.UtcNow;
        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Severity = ParseOrDefault(request.Severity, IncidentSeverity.SEV3),
            Status = IncidentStatus.Detected,
            StartedAt = now.AddMinutes(-15),
            DetectedAt = now,
            PrimaryService = request.PrimaryService.Trim(),
            CreatedByUserId = context.UserId,
            CommanderUserId = context.UserId,
            AffectedServicesCsv = JoinTags(request.AffectedServices),
            CreatedAt = now,
            UpdatedAt = now
        };

        incident.TimelineEvents.Add(new IncidentTimelineEvent
        {
            Id = Guid.NewGuid(),
            IncidentId = incident.Id,
            ActorUserId = context.UserId,
            EventType = "Detected",
            Summary = "Incident opened from operations console.",
            OccurredAt = now,
            CreatedAt = now
        });

        db.Incidents.Add(incident);
        AddGlobalAudit("Incident", incident.Id, context.UserId, "IncidentCreated", $"Created {incident.Severity} incident for {incident.PrimaryService}", now);
        AddOutbox("IncidentCreated", $$"""{"incidentId":"{{incident.Id}}","severity":"{{incident.Severity}}","service":"{{incident.PrimaryService}}"}""", now);
        await db.SaveChangesAsync(ct);
        return ToIncidentDetail(incident);
    }

    public async Task<IncidentDetailDto?> AddIncidentTimelineAsync(RequestContext context, Guid id, AddIncidentTimelineRequest request, CancellationToken ct)
    {
        if (!context.IsAdminOrEngineer)
        {
            throw new UnauthorizedAccessException("Only Admin and Engineer users can update incident timelines.");
        }

        var incident = await db.Incidents.Include(x => x.TimelineEvents).SingleOrDefaultAsync(x => x.Id == id, ct);
        if (incident is null)
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;
        incident.TimelineEvents.Add(new IncidentTimelineEvent
        {
            Id = Guid.NewGuid(),
            IncidentId = incident.Id,
            ActorUserId = context.UserId,
            EventType = request.EventType.Trim(),
            Summary = request.Summary.Trim(),
            IsAiGenerated = false,
            OccurredAt = now,
            CreatedAt = now
        });
        incident.UpdatedAt = now;
        incident.Version++;
        AddGlobalAudit("Incident", incident.Id, context.UserId, "TimelineEventAdded", request.Summary.Trim(), now);
        await db.SaveChangesAsync(ct);
        return ToIncidentDetail(incident);
    }

    public async Task<AgentRunDto?> RunIncidentInvestigationAsync(RequestContext context, Guid id, CancellationToken ct)
    {
        if (!context.IsAdminOrEngineer)
        {
            throw new UnauthorizedAccessException("Only Admin and Engineer users can run investigation agents.");
        }

        var incident = await db.Incidents.SingleOrDefaultAsync(x => x.Id == id, ct);
        if (incident is null)
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;
        var evidenceCount = await db.Logs.CountAsync(x => x.ServiceName == incident.PrimaryService && x.Timestamp >= incident.DetectedAt.AddHours(-2), ct);
        var agentRun = new AgentRun
        {
            Id = Guid.NewGuid(),
            AgentName = "IncidentInvestigationAgent",
            InputJson = $$"""{"incidentId":"{{incident.Id}}","service":"{{incident.PrimaryService}}"}""",
            OutputJson = $$"""{"rootCauseHypothesis":"Investigate {{incident.PrimaryService}} errors around detection window.","evidenceCount":{{evidenceCount}},"recommendedMitigation":"Follow linked runbook and validate rollback/queue pressure before risky action.","confidence":0.72}""",
            Status = "Completed",
            StartedAt = now,
            CompletedAt = now.AddSeconds(8),
            CreatedByUserId = context.UserId,
            RelatedIncidentId = incident.Id,
            TotalToolCalls = 3,
            TotalEstimatedTokens = 1800,
            EstimatedCost = 0.012m
        };
        agentRun.ToolCalls.Add(new Domain.Agents.AgentToolCall
        {
            Id = Guid.NewGuid(),
            AgentRunId = agentRun.Id,
            ToolName = "SearchLogs",
            InputJson = $$"""{"service":"{{incident.PrimaryService}}"}""",
            OutputJson = $$"""{"matchingLogs":{{evidenceCount}}}""",
            Status = "Succeeded",
            StartedAt = now,
            CompletedAt = now.AddSeconds(2)
        });
        db.AgentRuns.Add(agentRun);
        db.HumanApprovals.Add(new HumanApproval
        {
            Id = Guid.NewGuid(),
            RecommendationType = "IncidentMitigation",
            RecommendationJson = agentRun.OutputJson,
            Status = "Pending",
            RequestedByUserId = context.UserId,
            CreatedAt = now
        });
        incident.AiRootCauseCandidate = $"Agent hypothesis: review {incident.PrimaryService} errors and runbook mitigations.";
        incident.AiRootCauseStatus = "PendingApproval";
        incident.UpdatedAt = now;
        AddGlobalAudit("Incident", incident.Id, context.UserId, "AgentRunCreated", "Investigation agent completed and created a pending approval.", now);
        await db.SaveChangesAsync(ct);

        return ToAgentRunDto(agentRun);
    }

    public async Task<PagedResult<LogEntryDto>> SearchLogsAsync(RequestContext context, string? serviceName, string? level, string? query, int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);
        var logs = db.Logs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(serviceName))
        {
            logs = logs.Where(x => x.ServiceName == serviceName);
        }

        if (!string.IsNullOrWhiteSpace(level))
        {
            logs = logs.Where(x => x.Level == level);
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            logs = logs.Where(x => x.Message.Contains(query));
        }

        var total = await logs.CountAsync(ct);
        var items = await logs
            .OrderByDescending(x => x.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new LogEntryDto(x.Id, x.Timestamp, x.ServiceName, x.Environment, x.Level, x.Message, x.TraceId, x.CorrelationId, x.Host, x.ExceptionType, x.LinkedIncidentId))
            .ToListAsync(ct);
        return new PagedResult<LogEntryDto>(items, page, pageSize, total);
    }

    public async Task<IReadOnlyList<KnowledgeDocumentDto>> ListKnowledgeDocumentsAsync(RequestContext context, CancellationToken ct)
    {
        return await db.KnowledgeDocuments.AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(x => new KnowledgeDocumentDto(x.Id, x.Title, x.SourceType, SplitTags(x.TagsCsv), x.UpdatedAt))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<AgentRunDto>> ListAgentRunsAsync(RequestContext context, CancellationToken ct)
    {
        return await db.AgentRuns.AsNoTracking()
            .OrderByDescending(x => x.StartedAt)
            .Take(100)
            .Select(x => new AgentRunDto(x.Id, x.AgentName, x.Status, x.RelatedTicketId, x.RelatedIncidentId, x.TotalToolCalls, x.StartedAt))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<EvalRunDto>> ListEvalRunsAsync(RequestContext context, CancellationToken ct)
    {
        return await db.EvalRuns.AsNoTracking()
            .OrderByDescending(x => x.StartedAt)
            .Take(50)
            .Select(x => new EvalRunDto(x.Id, x.SuiteName, x.Status, x.TotalTests, x.PassedTests, x.FailedTests, x.AverageScore, x.StartedAt))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<AdminSettingDto>> ListAdminSettingsAsync(RequestContext context, CancellationToken ct)
    {
        if (context.Role != "Admin")
        {
            throw new UnauthorizedAccessException("Admin role required.");
        }

        return await db.AdminSettings.AsNoTracking()
            .OrderBy(x => x.Key)
            .Select(x => new AdminSettingDto(x.Id, x.Key, x.Value, x.Description, x.UpdatedAt))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<HumanApprovalDto>> ListApprovalsAsync(RequestContext context, CancellationToken ct)
    {
        return await db.HumanApprovals.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Take(100)
            .Select(x => new HumanApprovalDto(x.Id, x.RecommendationType, x.Status, x.CreatedAt))
            .ToListAsync(ct);
    }

    private static bool CanReadTicket(RequestContext context, Ticket ticket)
    {
        return context.IsAdminOrEngineer || ticket.CreatedByUserId == context.UserId || ticket.AssignedToUserId == context.UserId;
    }

    private static bool IsAllowedTicketTransition(TicketStatus current, TicketStatus next)
    {
        if (current == next)
        {
            return true;
        }

        return current switch
        {
            TicketStatus.Closed => false,
            TicketStatus.Resolved => next is TicketStatus.Closed or TicketStatus.InProgress,
            _ => true
        };
    }

    private static void EnsureSupportMutation(RequestContext context)
    {
        if (!context.CanMutateSupportWork)
        {
            throw new UnauthorizedAccessException("Support mutation role required.");
        }
    }

    private void AddGlobalAudit(string entityType, Guid entityId, Guid actorUserId, string eventType, string summary, DateTimeOffset now)
    {
        db.AuditEvents.Add(new AuditEvent
        {
            Id = Guid.NewGuid(),
            EntityType = entityType,
            EntityId = entityId,
            ActorUserId = actorUserId,
            EventType = eventType,
            Summary = summary,
            CreatedAt = now
        });
    }

    private void AddOutbox(string type, string payloadJson, DateTimeOffset now)
    {
        db.OutboxMessages.Add(new Domain.Operations.OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            PayloadJson = payloadJson,
            Status = "Pending",
            CreatedAt = now
        });
    }

    private static void AddTicketAudit(Ticket ticket, Guid actorUserId, string eventType, string summary, DateTimeOffset now)
    {
        ticket.AuditEvents.Add(new TicketAuditEvent
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            ActorUserId = actorUserId,
            EventType = eventType,
            Summary = summary,
            CreatedAt = now
        });
    }

    private static TicketDetailDto ToTicketDetail(Ticket ticket)
    {
        return new TicketDetailDto(
            ticket.Id,
            ticket.Title,
            ticket.Description,
            ticket.Status.ToString(),
            ticket.Priority.ToString(),
            ticket.Category.ToString(),
            ticket.CreatedByUserId,
            ticket.AssignedToUserId,
            ticket.RelatedIncidentId,
            SplitTags(ticket.TagsCsv),
            ticket.AiSummary,
            ticket.CreatedAt,
            ticket.UpdatedAt,
            ticket.ResolvedAt,
            ticket.Comments.OrderBy(x => x.CreatedAt).Select(x => new TicketCommentDto(x.Id, x.AuthorUserId, x.Body, x.IsInternal, x.CreatedAt)).ToList(),
            ticket.AuditEvents.OrderBy(x => x.CreatedAt).Select(x => new AuditEventDto(x.Id, x.EventType, x.Summary, x.CreatedAt)).ToList());
    }

    private static IncidentDetailDto ToIncidentDetail(Incident incident)
    {
        return new IncidentDetailDto(
            incident.Id,
            incident.Title,
            incident.Description,
            incident.Severity.ToString(),
            incident.Status.ToString(),
            incident.StartedAt,
            incident.DetectedAt,
            incident.ResolvedAt,
            incident.PrimaryService,
            incident.CreatedByUserId,
            incident.CommanderUserId,
            incident.RootCauseSummary,
            incident.CustomerImpact,
            incident.ResolutionSummary,
            SplitTags(incident.AffectedServicesCsv ?? string.Empty),
            incident.AiRootCauseCandidate,
            incident.AiRootCauseStatus,
            incident.TimelineEvents.OrderBy(x => x.OccurredAt).Select(x => new IncidentTimelineEventDto(x.Id, x.ActorUserId, x.EventType, x.Summary, x.IsAiGenerated, x.OccurredAt)).ToList());
    }

    private static AgentRunDto ToAgentRunDto(AgentRun run)
    {
        return new AgentRunDto(run.Id, run.AgentName, run.Status, run.RelatedTicketId, run.RelatedIncidentId, run.TotalToolCalls, run.StartedAt);
    }

    private static string JoinTags(IEnumerable<string> tags)
    {
        return string.Join(",", tags.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase));
    }

    private static string[] SplitTags(string tagsCsv)
    {
        return tagsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static bool TryParse<TEnum>(string? value, out TEnum result)
        where TEnum : struct
    {
        return Enum.TryParse(value, ignoreCase: true, out result);
    }

    private static TEnum ParseOrDefault<TEnum>(string? value, TEnum defaultValue)
        where TEnum : struct
    {
        return Enum.TryParse<TEnum>(value, ignoreCase: true, out var parsed) ? parsed : defaultValue;
    }
}
