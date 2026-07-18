using Microsoft.EntityFrameworkCore;
using NetAiOps.Application.Auth;
using NetAiOps.Domain.Agents;
using NetAiOps.Domain.Evaluations;
using NetAiOps.Domain.Incidents;
using NetAiOps.Domain.Knowledge;
using NetAiOps.Domain.Logs;
using NetAiOps.Domain.Operations;
using NetAiOps.Domain.Tickets;
using NetAiOps.Domain.Users;

namespace NetAiOps.Infrastructure.Persistence;

public sealed class DatabaseSeeder(NetAiOpsDbContext db, IPasswordHasher passwordHasher)
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        await db.Database.MigrateAsync(ct);

        var now = DateTimeOffset.UtcNow;
        var seedUsers = new[]
        {
            new SeedUser("admin@example.com", "Avery Admin", UserRole.Admin),
            new SeedUser("engineer@example.com", "Evan Engineer", UserRole.Engineer),
            new SeedUser("support@example.com", "Sam Support", UserRole.SupportAnalyst),
            new SeedUser("viewer@example.com", "Val Viewer", UserRole.Viewer)
        };

        foreach (var seedUser in seedUsers)
        {
            var existing = await db.Users.SingleOrDefaultAsync(x => x.Email == seedUser.Email, ct);
            if (existing is null)
            {
                db.Users.Add(new AppUser
                {
                    Id = Guid.NewGuid(),
                    Email = seedUser.Email,
                    DisplayName = seedUser.DisplayName,
                    PasswordHash = passwordHasher.Hash("Password123!"),
                    Role = seedUser.Role,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }
        }

        await db.SaveChangesAsync(ct);

        var admin = await db.Users.SingleAsync(x => x.Email == "admin@example.com", ct);
        var engineer = await db.Users.SingleAsync(x => x.Email == "engineer@example.com", ct);
        var support = await db.Users.SingleAsync(x => x.Email == "support@example.com", ct);

        if (!await db.Incidents.AnyAsync(ct))
        {
            var billingIncident = new Incident
            {
                Id = Guid.NewGuid(),
                Title = "BillingService timeout spike during checkout",
                Description = "Checkout requests intermittently fail with timeout errors. Database connection pool is saturated.",
                Severity = IncidentSeverity.SEV2,
                Status = IncidentStatus.Investigating,
                StartedAt = now.AddHours(-3),
                DetectedAt = now.AddHours(-2.5),
                PrimaryService = "BillingService",
                CreatedByUserId = engineer.Id,
                CommanderUserId = engineer.Id,
                CustomerImpact = "Some customers cannot complete checkout.",
                AffectedServicesCsv = "BillingService,DatabaseWorker,ApiGateway",
                AiRootCauseCandidate = "Likely database connection pool exhaustion based on timeout logs.",
                AiRootCauseStatus = "PendingApproval",
                CreatedAt = now.AddHours(-2.5),
                UpdatedAt = now.AddMinutes(-20)
            };
            billingIncident.TimelineEvents.Add(new IncidentTimelineEvent
            {
                Id = Guid.NewGuid(),
                IncidentId = billingIncident.Id,
                ActorUserId = engineer.Id,
                EventType = "Detected",
                Summary = "Checkout timeout alert fired from synthetic monitoring.",
                OccurredAt = now.AddHours(-2.5),
                CreatedAt = now.AddHours(-2.5)
            });
            billingIncident.TimelineEvents.Add(new IncidentTimelineEvent
            {
                Id = Guid.NewGuid(),
                IncidentId = billingIncident.Id,
                ActorUserId = engineer.Id,
                EventType = "MitigationSuggested",
                Summary = "Temporarily increase connection pool size and restart billing worker after checking slow queries.",
                IsAiGenerated = true,
                OccurredAt = now.AddMinutes(-40),
                CreatedAt = now.AddMinutes(-40)
            });

            var authIncident = new Incident
            {
                Id = Guid.NewGuid(),
                Title = "AuthService token validation failures",
                Description = "Login failures increased after signing key configuration drift.",
                Severity = IncidentSeverity.SEV3,
                Status = IncidentStatus.Mitigating,
                StartedAt = now.AddHours(-7),
                DetectedAt = now.AddHours(-6.8),
                PrimaryService = "AuthService",
                CreatedByUserId = engineer.Id,
                CommanderUserId = engineer.Id,
                RootCauseSummary = "Expired signing key config deployed to one environment.",
                CustomerImpact = "Small cohort of users saw intermittent login failures.",
                ResolutionSummary = "Rotating key config and redeploying AuthService.",
                AffectedServicesCsv = "AuthService,ApiGateway",
                CreatedAt = now.AddHours(-6.8),
                UpdatedAt = now.AddHours(-1)
            };
            authIncident.TimelineEvents.Add(new IncidentTimelineEvent
            {
                Id = Guid.NewGuid(),
                IncidentId = authIncident.Id,
                ActorUserId = engineer.Id,
                EventType = "Detected",
                Summary = "Login error-rate alert triggered.",
                OccurredAt = now.AddHours(-6.8),
                CreatedAt = now.AddHours(-6.8)
            });

            db.Incidents.AddRange(billingIncident, authIncident);

            db.Tickets.AddRange(
                CreateSeedTicket("Checkout fails with timeout", "Customers report checkout failures during payment authorization.", TicketPriority.Critical, TicketCategory.PaymentIssue, support.Id, engineer.Id, billingIncident.Id, "checkout,billing,customer-impact", now.AddHours(-2)),
                CreateSeedTicket("Login fails for some users", "Support reports token validation failures after deploy.", TicketPriority.High, TicketCategory.LoginIssue, support.Id, engineer.Id, authIncident.Id, "auth,login,deploy", now.AddHours(-6)),
                CreateSeedTicket("Search API slow for enterprise tenant", "Search endpoint latency exceeds 5 seconds for complex queries.", TicketPriority.High, TicketCategory.Performance, support.Id, engineer.Id, null, "search,latency,index", now.AddHours(-12)),
                CreateSeedTicket("Notification emails delayed", "Password reset and receipt emails delayed by queue backlog.", TicketPriority.Medium, TicketCategory.Unknown, support.Id, null, null, "notification,queue,email", now.AddDays(-1)));

            db.Logs.AddRange(CreateSeedLogs(now, billingIncident.Id, authIncident.Id));
        }

        if (!await db.KnowledgeDocuments.AnyAsync(ct))
        {
            db.KnowledgeDocuments.AddRange(
                CreateDoc("BillingService Timeout Runbook", "Runbook", "If BillingService timeouts spike, inspect database latency, connection pool saturation, retry policy, and slow checkout queries. Temporary mitigation: increase pool size and restart worker after draining in-flight jobs.", "billing,runbook,timeout", engineer.Id, now),
                CreateDoc("AuthService Runbook", "Runbook", "Token validation failures usually indicate signing key mismatch, clock skew, or stale configuration. Rotate key config and redeploy AuthService after validating issuer and audience.", "auth,runbook,jwt", engineer.Id, now),
                CreateDoc("Incident Response Handbook", "Runbook", "Declare severity, assign commander, establish timeline, communicate status, mitigate safely, and write postmortem after closure.", "incident,response,process", admin.Id, now),
                CreateDoc("Postmortem Template", "Postmortem", "Summary, impact, timeline, root cause, detection, resolution, what went well, what went wrong, action items.", "postmortem,template", admin.Id, now));
        }

        if (!await db.AgentRuns.AnyAsync(ct))
        {
            var run = new AgentRun
            {
                Id = Guid.NewGuid(),
                AgentName = "TicketTriageAgent",
                InputJson = """{"ticket":"Checkout fails with timeout"}""",
                OutputJson = """{"category":"PaymentIssue","priority":"Critical","confidence":0.84}""",
                Status = "Completed",
                StartedAt = now.AddHours(-1),
                CompletedAt = now.AddHours(-1).AddSeconds(5),
                CreatedByUserId = engineer.Id,
                TotalToolCalls = 2,
                TotalEstimatedTokens = 900,
                EstimatedCost = 0.006m
            };
            run.ToolCalls.Add(new AgentToolCall
            {
                Id = Guid.NewGuid(),
                AgentRunId = run.Id,
                ToolName = "SearchIncidents",
                InputJson = """{"service":"BillingService"}""",
                OutputJson = """{"matches":1}""",
                Status = "Succeeded",
                StartedAt = now.AddHours(-1),
                CompletedAt = now.AddHours(-1).AddSeconds(1)
            });
            db.AgentRuns.Add(run);
            db.HumanApprovals.Add(new HumanApproval
            {
                Id = Guid.NewGuid(),
                RecommendationType = "TicketPriorityChange",
                RecommendationJson = """{"priority":"Critical","reason":"Checkout failures linked to active incident"}""",
                Status = "Pending",
                RequestedByUserId = engineer.Id,
                CreatedAt = now.AddMinutes(-50)
            });
        }

        if (!await db.EvalRuns.AnyAsync(ct))
        {
            db.EvalRuns.AddRange(
                new EvalRun
                {
                    Id = Guid.NewGuid(),
                    SuiteName = "RAG Retrieval Smoke",
                    StartedAt = now.AddDays(-1),
                    CompletedAt = now.AddDays(-1).AddMinutes(2),
                    Status = "Passed",
                    TotalTests = 5,
                    PassedTests = 4,
                    FailedTests = 1,
                    AverageScore = 0.82,
                    Notes = "Billing timeout runbook retrieved at rank 1."
                },
                new EvalRun
                {
                    Id = Guid.NewGuid(),
                    SuiteName = "Agent Workflow Smoke",
                    StartedAt = now.AddHours(-6),
                    CompletedAt = now.AddHours(-6).AddMinutes(3),
                    Status = "Passed",
                    TotalTests = 4,
                    PassedTests = 3,
                    FailedTests = 1,
                    AverageScore = 0.76,
                    Notes = "Incident agent called expected log and runbook tools."
                });
        }

        if (!await db.AdminSettings.AnyAsync(ct))
        {
            db.AdminSettings.AddRange(
                new AdminSetting
                {
                    Id = Guid.NewGuid(),
                    Key = "Agents.RequireHumanApprovalForRiskyActions",
                    Value = "true",
                    Description = "Require approval before agents close incidents, create external tickets, or send incident broadcasts.",
                    UpdatedAt = now,
                    UpdatedByUserId = admin.Id
                },
                new AdminSetting
                {
                    Id = Guid.NewGuid(),
                    Key = "Logs.RetentionDays",
                    Value = "30",
                    Description = "Synthetic operational log retention period.",
                    UpdatedAt = now,
                    UpdatedByUserId = admin.Id
                },
                new AdminSetting
                {
                    Id = Guid.NewGuid(),
                    Key = "FeatureFlags.RagAssistant",
                    Value = "enabled",
                    Description = "Controls visibility of the RAG assistant experience.",
                    UpdatedAt = now,
                    UpdatedByUserId = admin.Id
                });
        }

        await db.SaveChangesAsync(ct);
    }

    private sealed record SeedUser(string Email, string DisplayName, UserRole Role);

    private static Ticket CreateSeedTicket(string title, string description, TicketPriority priority, TicketCategory category, Guid createdBy, Guid? assignedTo, Guid? incidentId, string tags, DateTimeOffset createdAt)
    {
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Status = priority == TicketPriority.Critical ? TicketStatus.Escalated : TicketStatus.Triaged,
            Priority = priority,
            Category = category,
            CreatedByUserId = createdBy,
            AssignedToUserId = assignedTo,
            RelatedIncidentId = incidentId,
            TagsCsv = tags,
            AiSummary = $"Seeded AI summary for {title}.",
            AiSuggestedPriority = priority.ToString(),
            AiSuggestedCategory = category.ToString(),
            CreatedAt = createdAt,
            UpdatedAt = createdAt.AddMinutes(25)
        };
        ticket.Comments.Add(new TicketComment
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            AuthorUserId = createdBy,
            Body = "Initial report captured by support.",
            IsInternal = false,
            CreatedAt = createdAt
        });
        ticket.AuditEvents.Add(new TicketAuditEvent
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            ActorUserId = createdBy,
            EventType = "TicketCreated",
            Summary = "Seed ticket created.",
            CreatedAt = createdAt
        });
        return ticket;
    }

    private static KnowledgeDocument CreateDoc(string title, string sourceType, string content, string tags, Guid createdBy, DateTimeOffset now)
    {
        return new KnowledgeDocument
        {
            Id = Guid.NewGuid(),
            Title = title,
            SourceType = sourceType,
            Content = content,
            TagsCsv = tags,
            CreatedByUserId = createdBy,
            CreatedAt = now.AddDays(-5),
            UpdatedAt = now.AddDays(-2)
        };
    }

    private static IEnumerable<SyntheticLog> CreateSeedLogs(DateTimeOffset now, Guid billingIncidentId, Guid authIncidentId)
    {
        var services = new[] { "BillingService", "AuthService", "SearchService", "NotificationService", "ApiGateway" };
        var logs = new List<SyntheticLog>();
        for (var i = 0; i < 80; i++)
        {
            var service = services[i % services.Length];
            var isBillingError = service == "BillingService" && i % 4 == 0;
            var isAuthError = service == "AuthService" && i % 7 == 0;
            logs.Add(new SyntheticLog
            {
                Id = Guid.NewGuid(),
                Timestamp = now.AddMinutes(-i * 6),
                ServiceName = service,
                Environment = "Production",
                Level = isBillingError || isAuthError ? (i % 12 == 0 ? "Critical" : "Error") : "Info",
                Message = isBillingError
                    ? "Timeout while acquiring database connection from pool during checkout."
                    : isAuthError
                        ? "Token validation failed because signing key configuration is expired."
                        : $"Processed request for {service}.",
                TraceId = $"trace-{i % 12:000}",
                CorrelationId = $"corr-{i:0000}",
                Host = $"ops-prod-{i % 6}",
                ExceptionType = isBillingError ? "TimeoutException" : isAuthError ? "SecurityTokenException" : null,
                MetadataJson = $$"""{"seeded":true,"sequence":{{i}}}""",
                LinkedIncidentId = isBillingError ? billingIncidentId : isAuthError ? authIncidentId : null
            });
        }

        return logs;
    }
}
