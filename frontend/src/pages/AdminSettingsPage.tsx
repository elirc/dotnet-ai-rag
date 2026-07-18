import { useQuery } from '@tanstack/react-query';
import { getAdminSettings } from '../app/api';
import { useAuthStore } from '../app/authStore';
import { EmptyState, PageScaffold } from './PageScaffold';

export function AdminSettingsPage() {
  const token = useAuthStore((state) => state.token);
  const query = useQuery({ queryKey: ['admin-settings'], queryFn: () => getAdminSettings(token!), enabled: Boolean(token) });

  if (query.isLoading) return <EmptyState message="Loading settings..." />;
  if (query.isError) return <EmptyState message={`Settings unavailable: ${query.error.message}`} />;

  return (
    <PageScaffold eyebrow="Administration" title="Settings">
      <div className="data-table settings">
        <div className="data-row header"><span>Key</span><span>Value</span><span>Description</span></div>
        {query.data!.map((setting) => (
          <div className="data-row" key={setting.id}>
            <strong>{setting.key}</strong>
            <span>{setting.value}</span>
            <span>{setting.description}</span>
          </div>
        ))}
      </div>
    </PageScaffold>
  );
}
