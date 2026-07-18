import { useQuery } from '@tanstack/react-query';
import { getDashboardSummary } from '../app/api';
import { useAuthStore } from '../app/authStore';

export function DashboardPage() {
  const token = useAuthStore((state) => state.token);
  const summaryQuery = useQuery({
    queryKey: ['dashboard-summary'],
    queryFn: () => getDashboardSummary(token!),
    enabled: Boolean(token),
  });

  if (summaryQuery.isLoading) {
    return <div className="state-box">Loading dashboard...</div>;
  }

  if (summaryQuery.isError) {
    return <div className="state-box error">Dashboard unavailable: {summaryQuery.error.message}</div>;
  }

  const summary = summaryQuery.data!;
  const cards = [
    ['Open tickets', summary.openTickets],
    ['Active incidents', summary.activeIncidents],
    ['Critical alerts', summary.criticalAlerts],
    ['Avg. resolution', `${summary.averageResolutionHours}h`],
    ['Agent runs today', summary.agentInvestigationsToday],
    ['Failed jobs', summary.failedBackgroundJobs],
    ['Eval pass rate', `${Math.round(summary.evalPassRate * 100)}%`],
  ];

  return (
    <section className="dashboard-grid">
      <div className="section-header">
        <div>
          <p className="eyebrow">Phase 1</p>
          <h2>Operational overview</h2>
        </div>
        <span className="status-pill">JWT protected</span>
      </div>
      <div className="metric-grid">
        {cards.map(([label, value]) => (
          <article className="metric-card" key={label}>
            <span>{label}</span>
            <strong>{value}</strong>
          </article>
        ))}
      </div>
      <section className="recommendations">
        <div className="section-header compact">
          <h2>Recent AI recommendations</h2>
          <span>{summary.recentAiRecommendations.length} drafts</span>
        </div>
        <div className="table">
          {summary.recentAiRecommendations.map((item) => (
            <div className="table-row" key={item.id}>
              <div>
                <strong>{item.title}</strong>
                <span>{item.source}</span>
              </div>
              <span>{item.status}</span>
            </div>
          ))}
        </div>
      </section>
    </section>
  );
}
