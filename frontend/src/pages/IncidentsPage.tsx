import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { getIncidents, runIncidentInvestigation } from '../app/api';
import { useAuthStore } from '../app/authStore';
import { EmptyState, PageScaffold } from './PageScaffold';

export function IncidentsPage() {
  const token = useAuthStore((state) => state.token);
  const user = useAuthStore((state) => state.user);
  const queryClient = useQueryClient();
  const query = useQuery({ queryKey: ['incidents'], queryFn: () => getIncidents(token!), enabled: Boolean(token) });
  const mutation = useMutation({
    mutationFn: (incidentId: string) => runIncidentInvestigation(token!, incidentId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['agent-runs'] }),
  });

  if (query.isLoading) return <EmptyState message="Loading incidents..." />;
  if (query.isError) return <EmptyState message={`Incidents unavailable: ${query.error.message}`} />;

  const canInvestigate = user?.role === 'Admin' || user?.role === 'Engineer';

  return (
    <PageScaffold eyebrow="Incident command" title="Incidents" action={<span className="status-pill">{query.data!.totalCount} total</span>}>
      <div className="data-table">
        <div className="data-row header"><span>Title</span><span>Severity</span><span>Status</span><span>Service</span><span>Action</span></div>
        {query.data!.items.map((incident) => (
          <div className="data-row" key={incident.id}>
            <strong>{incident.title}</strong>
            <span className={`badge ${incident.severity.toLowerCase()}`}>{incident.severity}</span>
            <span>{incident.status}</span>
            <span>{incident.primaryService}</span>
            <span>
              {canInvestigate ? (
                <button type="button" className="small-button" disabled={mutation.isPending} onClick={() => mutation.mutate(incident.id)}>
                  Run agent
                </button>
              ) : 'View only'}
            </span>
          </div>
        ))}
      </div>
    </PageScaffold>
  );
}
