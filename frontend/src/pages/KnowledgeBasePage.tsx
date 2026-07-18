import { useQuery } from '@tanstack/react-query';
import { getKnowledgeDocuments } from '../app/api';
import { useAuthStore } from '../app/authStore';
import { EmptyState, PageScaffold } from './PageScaffold';

export function KnowledgeBasePage() {
  const token = useAuthStore((state) => state.token);
  const query = useQuery({ queryKey: ['knowledge-documents'], queryFn: () => getKnowledgeDocuments(token!), enabled: Boolean(token) });

  if (query.isLoading) return <EmptyState message="Loading documents..." />;
  if (query.isError) return <EmptyState message={`Knowledge base unavailable: ${query.error.message}`} />;

  return (
    <PageScaffold eyebrow="Knowledge operations" title="Runbooks and documents" action={<span className="status-pill">{query.data!.length} docs</span>}>
      <div className="card-grid">
        {query.data!.map((doc) => (
          <article className="metric-card" key={doc.id}>
            <span>{doc.sourceType}</span>
            <strong className="card-title">{doc.title}</strong>
            <span>{doc.tags.join(', ')}</span>
          </article>
        ))}
      </div>
    </PageScaffold>
  );
}
