import { useQuery } from '@tanstack/react-query';
import { getAgentRuns, getApprovals } from '../app/api';
import { useAuthStore } from '../app/authStore';
import { EmptyState, PageScaffold } from './PageScaffold';

export function AgentsPage() {
  const token = useAuthStore((state) => state.token);
  const runs = useQuery({ queryKey: ['agent-runs'], queryFn: () => getAgentRuns(token!), enabled: Boolean(token) });
  const approvals = useQuery({ queryKey: ['approvals'], queryFn: () => getApprovals(token!), enabled: Boolean(token) });

  if (runs.isLoading || approvals.isLoading) return <EmptyState message="Loading agent operations..." />;
  if (runs.isError) return <EmptyState message={`Agent runs unavailable: ${runs.error.message}`} />;
  if (approvals.isError) return <EmptyState message={`Approvals unavailable: ${approvals.error.message}`} />;

  return (
    <PageScaffold eyebrow="Human-in-the-loop AI" title="Agents">
      <div className="split-grid">
        <section className="recommendations">
          <div className="section-header compact"><h2>Recent runs</h2><span>{runs.data!.length}</span></div>
          {runs.data!.map((run) => (
            <div className="table-row" key={run.id}>
              <div><strong>{run.agentName}</strong><span>{run.totalToolCalls} tool calls</span></div>
              <span>{run.status}</span>
            </div>
          ))}
        </section>
        <section className="recommendations">
          <div className="section-header compact"><h2>Approval queue</h2><span>{approvals.data!.length}</span></div>
          {approvals.data!.map((approval) => (
            <div className="table-row" key={approval.id}>
              <div><strong>{approval.recommendationType}</strong><span>{new Date(approval.createdAt).toLocaleString()}</span></div>
              <span>{approval.status}</span>
            </div>
          ))}
        </section>
      </div>
    </PageScaffold>
  );
}
