import { useQuery } from '@tanstack/react-query';
import { getEvalRuns } from '../app/api';
import { useAuthStore } from '../app/authStore';
import { EmptyState, PageScaffold } from './PageScaffold';

export function EvalsPage() {
  const token = useAuthStore((state) => state.token);
  const query = useQuery({ queryKey: ['eval-runs'], queryFn: () => getEvalRuns(token!), enabled: Boolean(token) });

  if (query.isLoading) return <EmptyState message="Loading eval runs..." />;
  if (query.isError) return <EmptyState message={`Eval runs unavailable: ${query.error.message}`} />;

  return (
    <PageScaffold eyebrow="Quality gates" title="Evaluation runs">
      <div className="data-table">
        <div className="data-row header"><span>Suite</span><span>Status</span><span>Pass</span><span>Fail</span><span>Score</span></div>
        {query.data!.map((run) => (
          <div className="data-row" key={run.id}>
            <strong>{run.suiteName}</strong>
            <span>{run.status}</span>
            <span>{run.passedTests}</span>
            <span>{run.failedTests}</span>
            <span>{Math.round(run.averageScore * 100)}%</span>
          </div>
        ))}
      </div>
    </PageScaffold>
  );
}
