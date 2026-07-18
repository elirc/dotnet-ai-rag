using Microsoft.EntityFrameworkCore;
using NetAiOps.Domain.Agents;
using NetAiOps.Domain.Evaluations;
using NetAiOps.Domain.Incidents;
using NetAiOps.Domain.Knowledge;
using NetAiOps.Domain.Logs;
using NetAiOps.Domain.Operations;
using NetAiOps.Domain.Tickets;
using NetAiOps.Domain.Users;

namespace NetAiOps.Infrastructure.Persistence;

public sealed class NetAiOpsDbContext(DbContextOptions<NetAiOpsDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();
    public DbSet<TicketAuditEvent> TicketAuditEvents => Set<TicketAuditEvent>();
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<IncidentTimelineEvent> IncidentTimelineEvents => Set<IncidentTimelineEvent>();
    public DbSet<SyntheticLog> Logs => Set<SyntheticLog>();
    public DbSet<KnowledgeDocument> KnowledgeDocuments => Set<KnowledgeDocument>();
    public DbSet<AgentRun> AgentRuns => Set<AgentRun>();
    public DbSet<AgentToolCall> AgentToolCalls => Set<AgentToolCall>();
    public DbSet<EvalRun> EvalRuns => Set<EvalRun>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<AdminSetting> AdminSettings => Set<AdminSetting>();
    public DbSet<HumanApproval> HumanApprovals => Set<HumanApproval>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Email).HasColumnName("email").HasMaxLength(256).IsRequired();
            entity.Property(x => x.DisplayName).HasColumnName("display_name").HasMaxLength(160).IsRequired();
            entity.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(512).IsRequired();
            entity.Property(x => x.Role)
                .HasColumnName("role")
                .HasConversion<string>()
                .HasMaxLength(64)
                .IsRequired();
            entity.Property(x => x.IsActive).HasColumnName("is_active");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("tickets");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.Status, x.Priority });
            entity.HasIndex(x => x.AssignedToUserId);
            entity.HasIndex(x => x.RelatedIncidentId);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Title).HasColumnName("title").HasMaxLength(160).IsRequired();
            entity.Property(x => x.Description).HasColumnName("description").HasMaxLength(4000).IsRequired();
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(64).IsRequired();
            entity.Property(x => x.Priority).HasColumnName("priority").HasConversion<string>().HasMaxLength(64).IsRequired();
            entity.Property(x => x.Category).HasColumnName("category").HasConversion<string>().HasMaxLength(64).IsRequired();
            entity.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(x => x.AssignedToUserId).HasColumnName("assigned_to_user_id");
            entity.Property(x => x.RelatedIncidentId).HasColumnName("related_incident_id");
            entity.Property(x => x.TagsCsv).HasColumnName("tags_csv").HasMaxLength(512).IsRequired();
            entity.Property(x => x.AiSummary).HasColumnName("ai_summary").HasMaxLength(1000);
            entity.Property(x => x.AiSuggestedPriority).HasColumnName("ai_suggested_priority").HasMaxLength(64);
            entity.Property(x => x.AiSuggestedCategory).HasColumnName("ai_suggested_category").HasMaxLength(64);
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
            entity.Property(x => x.ResolvedAt).HasColumnName("resolved_at");
            entity.Property(x => x.Version).HasColumnName("version").IsConcurrencyToken();

            entity.HasMany(x => x.Comments).WithOne().HasForeignKey(x => x.TicketId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(x => x.AuditEvents).WithOne().HasForeignKey(x => x.TicketId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TicketComment>(entity =>
        {
            entity.ToTable("ticket_comments");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.TicketId);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.TicketId).HasColumnName("ticket_id");
            entity.Property(x => x.AuthorUserId).HasColumnName("author_user_id");
            entity.Property(x => x.Body).HasColumnName("body").HasMaxLength(4000).IsRequired();
            entity.Property(x => x.IsInternal).HasColumnName("is_internal");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<TicketAuditEvent>(entity =>
        {
            entity.ToTable("ticket_audit_events");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.TicketId);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.TicketId).HasColumnName("ticket_id");
            entity.Property(x => x.ActorUserId).HasColumnName("actor_user_id");
            entity.Property(x => x.EventType).HasColumnName("event_type").HasMaxLength(80).IsRequired();
            entity.Property(x => x.Summary).HasColumnName("summary").HasMaxLength(1000).IsRequired();
            entity.Property(x => x.MetadataJson).HasColumnName("metadata_json");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.ToTable("incidents");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.Status, x.Severity });
            entity.HasIndex(x => x.PrimaryService);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Title).HasColumnName("title").HasMaxLength(180).IsRequired();
            entity.Property(x => x.Description).HasColumnName("description").HasMaxLength(4000).IsRequired();
            entity.Property(x => x.Severity).HasColumnName("severity").HasConversion<string>().HasMaxLength(32);
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(64);
            entity.Property(x => x.StartedAt).HasColumnName("started_at");
            entity.Property(x => x.DetectedAt).HasColumnName("detected_at");
            entity.Property(x => x.ResolvedAt).HasColumnName("resolved_at");
            entity.Property(x => x.PrimaryService).HasColumnName("primary_service").HasMaxLength(120).IsRequired();
            entity.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(x => x.CommanderUserId).HasColumnName("commander_user_id");
            entity.Property(x => x.RootCauseSummary).HasColumnName("root_cause_summary").HasMaxLength(2000);
            entity.Property(x => x.CustomerImpact).HasColumnName("customer_impact").HasMaxLength(2000);
            entity.Property(x => x.ResolutionSummary).HasColumnName("resolution_summary").HasMaxLength(2000);
            entity.Property(x => x.AffectedServicesCsv).HasColumnName("affected_services_csv").HasMaxLength(512);
            entity.Property(x => x.AiRootCauseCandidate).HasColumnName("ai_root_cause_candidate").HasMaxLength(2000);
            entity.Property(x => x.AiRootCauseStatus).HasColumnName("ai_root_cause_status").HasMaxLength(64);
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            entity.Property(x => x.Version).HasColumnName("version").IsConcurrencyToken();
            entity.HasMany(x => x.TimelineEvents).WithOne().HasForeignKey(x => x.IncidentId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<IncidentTimelineEvent>(entity =>
        {
            entity.ToTable("incident_timeline_events");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.IncidentId);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.IncidentId).HasColumnName("incident_id");
            entity.Property(x => x.ActorUserId).HasColumnName("actor_user_id");
            entity.Property(x => x.EventType).HasColumnName("event_type").HasMaxLength(80).IsRequired();
            entity.Property(x => x.Summary).HasColumnName("summary").HasMaxLength(1200).IsRequired();
            entity.Property(x => x.IsAiGenerated).HasColumnName("is_ai_generated");
            entity.Property(x => x.OccurredAt).HasColumnName("occurred_at");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<SyntheticLog>(entity =>
        {
            entity.ToTable("logs");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.ServiceName, x.Level, x.Timestamp });
            entity.HasIndex(x => x.TraceId);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Timestamp).HasColumnName("timestamp");
            entity.Property(x => x.ServiceName).HasColumnName("service_name").HasMaxLength(120);
            entity.Property(x => x.Environment).HasColumnName("environment").HasMaxLength(40);
            entity.Property(x => x.Level).HasColumnName("level").HasMaxLength(40);
            entity.Property(x => x.Message).HasColumnName("message").HasMaxLength(2000);
            entity.Property(x => x.TraceId).HasColumnName("trace_id").HasMaxLength(80);
            entity.Property(x => x.CorrelationId).HasColumnName("correlation_id").HasMaxLength(80);
            entity.Property(x => x.Host).HasColumnName("host").HasMaxLength(120);
            entity.Property(x => x.ExceptionType).HasColumnName("exception_type").HasMaxLength(200);
            entity.Property(x => x.StackTrace).HasColumnName("stack_trace");
            entity.Property(x => x.MetadataJson).HasColumnName("metadata_json");
            entity.Property(x => x.LinkedIncidentId).HasColumnName("linked_incident_id");
        });

        modelBuilder.Entity<KnowledgeDocument>(entity =>
        {
            entity.ToTable("knowledge_documents");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.SourceType);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
            entity.Property(x => x.SourceType).HasColumnName("source_type").HasMaxLength(80).IsRequired();
            entity.Property(x => x.Content).HasColumnName("content").IsRequired();
            entity.Property(x => x.FileName).HasColumnName("file_name").HasMaxLength(260);
            entity.Property(x => x.TagsCsv).HasColumnName("tags_csv").HasMaxLength(512);
            entity.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<AgentRun>(entity =>
        {
            entity.ToTable("agent_runs");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.AgentName, x.Status, x.StartedAt });
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.AgentName).HasColumnName("agent_name").HasMaxLength(120);
            entity.Property(x => x.InputJson).HasColumnName("input_json");
            entity.Property(x => x.OutputJson).HasColumnName("output_json");
            entity.Property(x => x.Status).HasColumnName("status").HasMaxLength(64);
            entity.Property(x => x.StartedAt).HasColumnName("started_at");
            entity.Property(x => x.CompletedAt).HasColumnName("completed_at");
            entity.Property(x => x.ErrorMessage).HasColumnName("error_message").HasMaxLength(2000);
            entity.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(x => x.RelatedTicketId).HasColumnName("related_ticket_id");
            entity.Property(x => x.RelatedIncidentId).HasColumnName("related_incident_id");
            entity.Property(x => x.TotalToolCalls).HasColumnName("total_tool_calls");
            entity.Property(x => x.TotalEstimatedTokens).HasColumnName("total_estimated_tokens");
            entity.Property(x => x.EstimatedCost).HasColumnName("estimated_cost").HasPrecision(12, 6);
            entity.HasMany(x => x.ToolCalls).WithOne().HasForeignKey(x => x.AgentRunId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AgentToolCall>(entity =>
        {
            entity.ToTable("agent_tool_calls");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.AgentRunId).HasColumnName("agent_run_id");
            entity.Property(x => x.ToolName).HasColumnName("tool_name").HasMaxLength(120);
            entity.Property(x => x.InputJson).HasColumnName("input_json");
            entity.Property(x => x.OutputJson).HasColumnName("output_json");
            entity.Property(x => x.Status).HasColumnName("status").HasMaxLength(64);
            entity.Property(x => x.StartedAt).HasColumnName("started_at");
            entity.Property(x => x.CompletedAt).HasColumnName("completed_at");
            entity.Property(x => x.ErrorMessage).HasColumnName("error_message").HasMaxLength(2000);
        });

        modelBuilder.Entity<EvalRun>(entity =>
        {
            entity.ToTable("eval_runs");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.SuiteName, x.StartedAt });
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.SuiteName).HasColumnName("suite_name").HasMaxLength(160);
            entity.Property(x => x.StartedAt).HasColumnName("started_at");
            entity.Property(x => x.CompletedAt).HasColumnName("completed_at");
            entity.Property(x => x.Status).HasColumnName("status").HasMaxLength(64);
            entity.Property(x => x.TotalTests).HasColumnName("total_tests");
            entity.Property(x => x.PassedTests).HasColumnName("passed_tests");
            entity.Property(x => x.FailedTests).HasColumnName("failed_tests");
            entity.Property(x => x.AverageScore).HasColumnName("average_score");
            entity.Property(x => x.Notes).HasColumnName("notes").HasMaxLength(2000);
        });

        modelBuilder.Entity<AuditEvent>(entity =>
        {
            entity.ToTable("audit_events");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.EntityType, x.EntityId, x.CreatedAt });
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.EntityType).HasColumnName("entity_type").HasMaxLength(80);
            entity.Property(x => x.EntityId).HasColumnName("entity_id");
            entity.Property(x => x.ActorUserId).HasColumnName("actor_user_id");
            entity.Property(x => x.EventType).HasColumnName("event_type").HasMaxLength(80);
            entity.Property(x => x.Summary).HasColumnName("summary").HasMaxLength(1000);
            entity.Property(x => x.MetadataJson).HasColumnName("metadata_json");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("outbox_messages");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.Status, x.CreatedAt });
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Type).HasColumnName("type").HasMaxLength(120);
            entity.Property(x => x.PayloadJson).HasColumnName("payload_json");
            entity.Property(x => x.Status).HasColumnName("status").HasMaxLength(64);
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.Property(x => x.ProcessedAt).HasColumnName("processed_at");
            entity.Property(x => x.ErrorMessage).HasColumnName("error_message").HasMaxLength(2000);
        });

        modelBuilder.Entity<AdminSetting>(entity =>
        {
            entity.ToTable("admin_settings");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Key).IsUnique();
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Key).HasColumnName("key").HasMaxLength(120);
            entity.Property(x => x.Value).HasColumnName("value").HasMaxLength(1000);
            entity.Property(x => x.Description).HasColumnName("description").HasMaxLength(1000);
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            entity.Property(x => x.UpdatedByUserId).HasColumnName("updated_by_user_id");
        });

        modelBuilder.Entity<HumanApproval>(entity =>
        {
            entity.ToTable("human_approvals");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.Status, x.CreatedAt });
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.RecommendationType).HasColumnName("recommendation_type").HasMaxLength(120);
            entity.Property(x => x.RecommendationJson).HasColumnName("recommendation_json");
            entity.Property(x => x.Status).HasColumnName("status").HasMaxLength(64);
            entity.Property(x => x.RequestedByUserId).HasColumnName("requested_by_user_id");
            entity.Property(x => x.DecidedByUserId).HasColumnName("decided_by_user_id");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.Property(x => x.DecidedAt).HasColumnName("decided_at");
        });
    }
}
