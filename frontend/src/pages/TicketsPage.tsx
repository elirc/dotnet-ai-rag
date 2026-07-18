import { useQuery } from '@tanstack/react-query';
import { getTickets } from '../app/api';
import { useAuthStore } from '../app/authStore';
import { EmptyState, PageScaffold } from './PageScaffold';

export function TicketsPage() {
  const token = useAuthStore((state) => state.token);
  const query = useQuery({ queryKey: ['tickets'], queryFn: () => getTickets(token!), enabled: Boolean(token) });

  if (query.isLoading) return <EmptyState message="Loading tickets..." />;
  if (query.isError) return <EmptyState message={`Tickets unavailable: ${query.error.message}`} />;

  return (
    <PageScaffold eyebrow="Support operations" title="Tickets" action={<span className="status-pill">{query.data!.totalCount} total</span>}>
      <div className="data-table">
        <div className="data-row header"><span>Title</span><span>Status</span><span>Priority</span><span>Category</span><span>Tags</span></div>
        {query.data!.items.map((ticket) => (
          <div className="data-row" key={ticket.id}>
            <strong>{ticket.title}</strong>
            <span>{ticket.status}</span>
            <span className={`badge ${ticket.priority.toLowerCase()}`}>{ticket.priority}</span>
            <span>{ticket.category}</span>
            <span>{ticket.tags.join(', ') || 'none'}</span>
          </div>
        ))}
      </div>
    </PageScaffold>
  );
}
