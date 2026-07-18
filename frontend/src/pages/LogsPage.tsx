import { useQuery } from '@tanstack/react-query';
import { getLogs } from '../app/api';
import { useAuthStore } from '../app/authStore';
import { EmptyState, PageScaffold } from './PageScaffold';

export function LogsPage() {
  const token = useAuthStore((state) => state.token);
  const query = useQuery({ queryKey: ['logs'], queryFn: () => getLogs(token!), enabled: Boolean(token) });

  if (query.isLoading) return <EmptyState message="Loading logs..." />;
  if (query.isError) return <EmptyState message={`Logs unavailable: ${query.error.message}`} />;

  return (
    <PageScaffold eyebrow="Observability" title="Synthetic logs" action={<span className="status-pill">{query.data!.totalCount} matching</span>}>
      <div className="data-table logs">
        <div className="data-row header"><span>Time</span><span>Level</span><span>Service</span><span>Trace</span><span>Message</span></div>
        {query.data!.items.map((log) => (
          <div className="data-row" key={log.id}>
            <span>{new Date(log.timestamp).toLocaleTimeString()}</span>
            <span className={`badge ${log.level.toLowerCase()}`}>{log.level}</span>
            <span>{log.serviceName}</span>
            <span>{log.traceId}</span>
            <span>{log.message}</span>
          </div>
        ))}
      </div>
    </PageScaffold>
  );
}
