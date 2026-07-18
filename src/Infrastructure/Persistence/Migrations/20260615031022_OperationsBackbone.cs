using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetAiOps.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OperationsBackbone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    value = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_by_user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "agent_runs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    agent_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    input_json = table.Column<string>(type: "text", nullable: false),
                    output_json = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    related_ticket_id = table.Column<Guid>(type: "uuid", nullable: true),
                    related_incident_id = table.Column<Guid>(type: "uuid", nullable: true),
                    total_tool_calls = table.Column<int>(type: "integer", nullable: false),
                    total_estimated_tokens = table.Column<int>(type: "integer", nullable: false),
                    estimated_cost = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_runs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audit_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actor_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    summary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    metadata_json = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "eval_runs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    suite_name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    total_tests = table.Column<int>(type: "integer", nullable: false),
                    passed_tests = table.Column<int>(type: "integer", nullable: false),
                    failed_tests = table.Column<int>(type: "integer", nullable: false),
                    average_score = table.Column<double>(type: "double precision", nullable: false),
                    notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_runs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "human_approvals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    recommendation_type = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    recommendation_json = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    requested_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    decided_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    decided_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_human_approvals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "incidents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    severity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    detected_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    resolved_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    primary_service = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    commander_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    root_cause_summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    customer_impact = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    resolution_summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    affected_services_csv = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    ai_root_cause_candidate = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ai_root_cause_status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incidents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    source_type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    file_name = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: true),
                    tags_csv = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_documents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    service_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    environment = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    level = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    trace_id = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    correlation_id = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    host = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    exception_type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    stack_trace = table.Column<string>(type: "text", nullable: true),
                    metadata_json = table.Column<string>(type: "text", nullable: true),
                    linked_incident_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    payload_json = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    priority = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    category = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_to_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    related_incident_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tags_csv = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ai_summary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ai_suggested_priority = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ai_suggested_category = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    resolved_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "agent_tool_calls",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    agent_run_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tool_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    input_json = table.Column<string>(type: "text", nullable: false),
                    output_json = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    error_message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_tool_calls", x => x.id);
                    table.ForeignKey(
                        name: "FK_agent_tool_calls_agent_runs_agent_run_id",
                        column: x => x.agent_run_id,
                        principalTable: "agent_runs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incident_timeline_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    incident_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actor_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    summary = table.Column<string>(type: "character varying(1200)", maxLength: 1200, nullable: false),
                    is_ai_generated = table.Column<bool>(type: "boolean", nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incident_timeline_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_incident_timeline_events_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ticket_audit_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actor_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    summary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    metadata_json = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_audit_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_ticket_audit_events_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ticket_comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    body = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    is_internal = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_ticket_comments_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_admin_settings_key",
                table: "admin_settings",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_agent_runs_agent_name_status_started_at",
                table: "agent_runs",
                columns: new[] { "agent_name", "status", "started_at" });

            migrationBuilder.CreateIndex(
                name: "IX_agent_tool_calls_agent_run_id",
                table: "agent_tool_calls",
                column: "agent_run_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_entity_type_entity_id_created_at",
                table: "audit_events",
                columns: new[] { "entity_type", "entity_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_eval_runs_suite_name_started_at",
                table: "eval_runs",
                columns: new[] { "suite_name", "started_at" });

            migrationBuilder.CreateIndex(
                name: "IX_human_approvals_status_created_at",
                table: "human_approvals",
                columns: new[] { "status", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_incident_timeline_events_incident_id",
                table: "incident_timeline_events",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "IX_incidents_primary_service",
                table: "incidents",
                column: "primary_service");

            migrationBuilder.CreateIndex(
                name: "IX_incidents_status_severity",
                table: "incidents",
                columns: new[] { "status", "severity" });

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_documents_source_type",
                table: "knowledge_documents",
                column: "source_type");

            migrationBuilder.CreateIndex(
                name: "IX_logs_service_name_level_timestamp",
                table: "logs",
                columns: new[] { "service_name", "level", "timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_logs_trace_id",
                table: "logs",
                column: "trace_id");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_messages_status_created_at",
                table: "outbox_messages",
                columns: new[] { "status", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_ticket_audit_events_ticket_id",
                table: "ticket_audit_events",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_comments_ticket_id",
                table: "ticket_comments",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_assigned_to_user_id",
                table: "tickets",
                column: "assigned_to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_related_incident_id",
                table: "tickets",
                column: "related_incident_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_status_priority",
                table: "tickets",
                columns: new[] { "status", "priority" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_settings");

            migrationBuilder.DropTable(
                name: "agent_tool_calls");

            migrationBuilder.DropTable(
                name: "audit_events");

            migrationBuilder.DropTable(
                name: "eval_runs");

            migrationBuilder.DropTable(
                name: "human_approvals");

            migrationBuilder.DropTable(
                name: "incident_timeline_events");

            migrationBuilder.DropTable(
                name: "knowledge_documents");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "outbox_messages");

            migrationBuilder.DropTable(
                name: "ticket_audit_events");

            migrationBuilder.DropTable(
                name: "ticket_comments");

            migrationBuilder.DropTable(
                name: "agent_runs");

            migrationBuilder.DropTable(
                name: "incidents");

            migrationBuilder.DropTable(
                name: "tickets");
        }
    }
}
